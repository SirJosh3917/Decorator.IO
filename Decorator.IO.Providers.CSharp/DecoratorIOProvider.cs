using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Decorator.IO.Core.Tokens;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Decorator.IO.Providers.CSharp
{
	public class DecoratorIOProvider : IRoslynCodeProvider
	{
		private readonly Model[] _models;
		private readonly IVariableNameProvider _variableNameProvider;
		private IEnumerable<MemberDeclarationSyntax> _serializeMethods;
		private IEnumerable<MemberDeclarationSyntax> _deserializeMethods;

		public DecoratorIOProvider(Model[] models, IVariableNameProvider variableNameProvider)
		{
			_models = models;
			_variableNameProvider = variableNameProvider;

			_serializeMethods =
				models.Select(x => new DecoratorIOSerializeProvider(x, _variableNameProvider))
					.Select(x => x.Provide());

			_deserializeMethods =
				models.Select(x => new DecoratorIODeserializeProvider(x, _variableNameProvider))
					.Select(x => x.Provide());
		}

		public MemberDeclarationSyntax Provide()
		{
			return ClassDeclaration("DecoratorIO")
				.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword)))
				.WithMembers(List(_serializeMethods.Concat(_deserializeMethods)));
		}
	}

	public class DecoratorIOSerializeProvider : IRoslynCodeProvider
	{
		private readonly Model _model;
		private readonly IVariableNameProvider _variableNameProvider;
		private Field[] _fields;

		public DecoratorIOSerializeProvider(Model model, IVariableNameProvider variableNameProvider)
		{
			_model = model;
			_variableNameProvider = variableNameProvider;
			_fields = new ModelFlattener(_model).FlattenToFields();
		}

		public MemberDeclarationSyntax Provide()
		{
			var size = _variableNameProvider.New();
			var array = _variableNameProvider.New();
			var instance = _variableNameProvider.New();
			var position = _variableNameProvider.New();

			var serialize = new SerializeBodyProvider(_model, _variableNameProvider, size, array, instance, position);
			var code = serialize.Provide();

			return MethodDeclaration
			(
				ArrayType(PredefinedType(Token(SyntaxKind.ObjectKeyword)))
					.WithRankSpecifiers(SingletonList(ArrayRankSpecifier(SingletonSeparatedList<ExpressionSyntax>(OmittedArraySizeExpression())))),
				Identifier($"Serialize{_model.Identifier}")
			)
				.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword)))
				.WithParameterList(ParameterList(SingletonSeparatedList(
					Parameter(Identifier(instance))
						.WithType(IdentifierName(_model.Identifier))
					)))
				.WithBody(code);
		}
	}

	public class SerializeBodyProvider
	{
		private readonly Model _model;
		private readonly IVariableNameProvider _variableNameProvider;
		private readonly string _sizeName;
		private readonly string _arrayName;
		private readonly string _instanceName;
		private readonly string _positionName;

		public SerializeBodyProvider(Model model, IVariableNameProvider variableNameProvider, string sizeName, string arrayName, string instanceName, string positionName)
		{
			_model = model;
			_variableNameProvider = variableNameProvider;
			_sizeName = sizeName;
			_arrayName = arrayName;
			_instanceName = instanceName;
			_positionName = positionName;
		}

		public BlockSyntax Provide()
		{
			var fields = new ModelFlattener(_model).FlattenToFields();

			var size = IdentifierName(_sizeName);
			var array = IdentifierName(_arrayName);
			var instance = IdentifierName(_instanceName);
			var position = IdentifierName(_positionName);

			var sizeIdent = Identifier(_sizeName);
			var arrayIdent = Identifier(_arrayName);
			var positionIdent = Identifier(_positionName);

			BlockSyntax sizeInit = Block();
			BlockSyntax serialize = Block();
			Field last = null;

			foreach (var field in fields)
			{

				if (last != null)
				{
					var incPosBy = field.Position - last.Position - 1;
					sizeInit = sizeInit.AddStatements
					(
						ExpressionStatement
						(
							AssignmentExpression
							(
								SyntaxKind.AddAssignmentExpression,
								size,
								LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(incPosBy))
							)
						)
					);
				}

				sizeInit = sizeInit.AddStatements(ProvideSizeFor(size, array, _variableNameProvider, field).Statements
					.ToArray());

				if (last != null)
				{
					var incPosBy = field.Position - last.Position - 1;
					serialize = serialize.AddStatements
					(
						ExpressionStatement
						(
							AssignmentExpression
							(
								SyntaxKind.AddAssignmentExpression,
								position,
								LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(incPosBy))
							)
						)
					);
				}

				last = field;

				serialize = serialize.AddStatements(
					ProvideSerializeFor(array, position, instance, _variableNameProvider, field)
						.Statements.ToArray());
			}

			var code = Block();

			var initSizeVar = LocalDeclarationStatement(
				VariableDeclaration(PredefinedType(Token(SyntaxKind.IntKeyword)))
					.WithVariables(SingletonSeparatedList
					(
						VariableDeclarator(sizeIdent)
							.WithInitializer(EqualsValueClause(LiteralExpression(SyntaxKind.NumericLiteralExpression,
								Literal(0))))
					)));

			var initPositionVar = LocalDeclarationStatement(
				VariableDeclaration(PredefinedType(Token(SyntaxKind.IntKeyword)))
					.WithVariables(SingletonSeparatedList
					(
						VariableDeclarator(positionIdent)
							.WithInitializer(EqualsValueClause(LiteralExpression(SyntaxKind.NumericLiteralExpression,
								Literal(0))))
					)));

			var initObjArray = LocalDeclarationStatement(VariableDeclaration(
				ArrayType(PredefinedType(Token(SyntaxKind.ObjectKeyword)))
					.WithRankSpecifiers(SingletonList(
						ArrayRankSpecifier(
							SingletonSeparatedList<ExpressionSyntax>(OmittedArraySizeExpression()))))).WithVariables(
				SingletonSeparatedList(VariableDeclarator(arrayIdent).WithInitializer(
					EqualsValueClause(ArrayCreationExpression(ArrayType(PredefinedType(Token(SyntaxKind.ObjectKeyword)))
						.WithRankSpecifiers(SingletonList(
							ArrayRankSpecifier(
								SingletonSeparatedList<ExpressionSyntax>(
									size))))))))));

			//var returnTrue =
			//	Block(SingletonList<StatementSyntax>(ReturnStatement(LiteralExpression(SyntaxKind.TrueKeyword))));

			code = code.AddStatements(initSizeVar);
			code = code.AddStatements(sizeInit.Statements.ToArray());
			code = code.AddStatements(initObjArray);
			code = code.AddStatements(initPositionVar);
			code = code.AddStatements(serialize.Statements.ToArray());
			code = code.AddStatements(ReturnStatement(array));

			return code;
		}

		public static BlockSyntax ProvideSizeFor
		(
			IdentifierNameSyntax size,
			IdentifierNameSyntax array,
			IVariableNameProvider variableNameProvider,
			Field field
		)
		{
			var codeProvider = GetCodeProvider(variableNameProvider, field.Modifier);

			return codeProvider.SerializeSize(size);
		}

		public static BlockSyntax ProvideSerializeFor
		(
			IdentifierNameSyntax array,
			IdentifierNameSyntax position,
			IdentifierNameSyntax instance,
			IVariableNameProvider variableNameProvider,
			Field field
		)
		{
			var codeProvider = GetCodeProvider(variableNameProvider, field.Modifier);

			var member = MemberAccessExpression
			(
				SyntaxKind.SimpleMemberAccessExpression,
				instance,
				IdentifierName(field.Identifier)
			);

			return codeProvider.Serialize(member, array, position);
		}

		private static ICodeProvider GetCodeProvider(IVariableNameProvider variableNameProvider, Modifier modifier)
		{
			return typeof(SerializeBodyProvider).Assembly.GetTypes()
				.Where(x => x.GetInterfaces().Contains(typeof(ICodeProvider)))
				.Where(x =>
				{
					var attribute = x.GetCustomAttribute<CodeProviderAttribute>(true);
					return attribute != null && attribute.Modifier == modifier;
				})
				.Select(x => x.GetConstructors().First().Invoke(new object[] { variableNameProvider }))
				.Cast<ICodeProvider>()
				.First();
		}
	}


	public class DecoratorIODeserializeProvider : IRoslynCodeProvider
	{
		private readonly Model _model;
		private readonly IVariableNameProvider _variableNameProvider;
		private Field[] _fields;

		public DecoratorIODeserializeProvider(Model model, IVariableNameProvider variableNameProvider)
		{
			_model = model;
			_variableNameProvider = variableNameProvider;
			_fields = new ModelFlattener(_model).FlattenToFields();
		}

		public MemberDeclarationSyntax Provide()
		{
			var array = _variableNameProvider.New();
			var instance = _variableNameProvider.New();
			var position = _variableNameProvider.New();

			var serialize = new DeserializeBodyProvider(_model, _variableNameProvider, array, instance, position);
			var code = serialize.Provide();

			return MethodDeclaration
				(
					PredefinedType(Token(SyntaxKind.BoolKeyword)),
					Identifier($"TryDeserialize{_model.Identifier}")
				)
				.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword)))
				.WithParameterList(ParameterList(SeparatedList<ParameterSyntax>(new SyntaxNodeOrToken[]
				{
					Parameter(Identifier(array)).WithType(ArrayType(PredefinedType(Token(SyntaxKind.ObjectKeyword)))
						.WithRankSpecifiers(SingletonList(
							ArrayRankSpecifier(
								SingletonSeparatedList<ExpressionSyntax>(OmittedArraySizeExpression()))))),
					Token(SyntaxKind.CommaToken),
					Parameter(Identifier(instance)).WithModifiers(TokenList(Token(SyntaxKind.OutKeyword)))
						.WithType(IdentifierName(_model.Identifier))
				})))
				.WithBody(code);
		}
	}

	public class DeserializeBodyProvider
	{
		private readonly Model _model;
		private readonly IVariableNameProvider _variableNameProvider;
		private readonly string _arrayName;
		private readonly string _instanceName;
		private readonly string _positionName;

		public DeserializeBodyProvider(Model model, IVariableNameProvider variableNameProvider, string arrayName, string instanceName, string positionName){
			_model = model;
			_variableNameProvider = variableNameProvider;
			_arrayName = arrayName;
			_instanceName = instanceName;
			_positionName = positionName;
		}

		public BlockSyntax Provide()
		{
			var fields = new ModelFlattener(_model).FlattenToFields();

			var array = IdentifierName(_arrayName);
			var instance = IdentifierName(_instanceName);
			var position = IdentifierName(_positionName);

			var arrayIdent = Identifier(_arrayName);
			var positionIdent = Identifier(_positionName);

			BlockSyntax serialize = Block();
			Field last = null;

			foreach (var field in fields)
			{
				if (last != null)
				{
					var incPosBy = field.Position - last.Position - 1;
					serialize = serialize.AddStatements
					(
						ExpressionStatement
						(
							AssignmentExpression
							(
								SyntaxKind.AddAssignmentExpression,
								position,
								LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(incPosBy))
							)
						)
					);
				}

				serialize = serialize.AddStatements(
					ProvideDeserializeFor(array, position, instance, _variableNameProvider, field)
						.Statements.ToArray());

				last = field;
			}

			var code = Block();

			var initExpression = ExpressionStatement(AssignmentExpression(
				SyntaxKind.SimpleAssignmentExpression,
				instance,
				ObjectCreationExpression(IdentifierName(_model.Identifier))
					.WithArgumentList(ArgumentList())));

			var initPositionVar = LocalDeclarationStatement(
				VariableDeclaration(PredefinedType(Token(SyntaxKind.IntKeyword)))
					.WithVariables(SingletonSeparatedList
					(
						VariableDeclarator(positionIdent)
							.WithInitializer(EqualsValueClause(LiteralExpression(SyntaxKind.NumericLiteralExpression,
								Literal(0))))
					)));

			//var returnTrue =
			//	Block(SingletonList<StatementSyntax>(ReturnStatement(LiteralExpression(SyntaxKind.TrueKeyword))));

			code = code.AddStatements(initPositionVar);
			code = code.AddStatements(initExpression);
			code = code.AddStatements(serialize.Statements.ToArray());
			code = code.AddStatements(ReturnStatement(LiteralExpression(SyntaxKind.TrueLiteralExpression)));

			return code;
		}

		public static BlockSyntax ProvideDeserializeFor
		(
			IdentifierNameSyntax array,
			IdentifierNameSyntax position,
			IdentifierNameSyntax instance,
			IVariableNameProvider variableNameProvider,
			Field field
		)
		{
			var codeProvider = GetCodeProvider(variableNameProvider, field.Modifier);

			var member = MemberAccessExpression
			(
				SyntaxKind.SimpleMemberAccessExpression,
				instance,
				IdentifierName(field.Identifier)
			);

			return codeProvider.Deserialize(field.Type.IsValueType, (IdentifierName(field.Type.Identifier)), member, array, position);
		}

		private static ICodeProvider GetCodeProvider(IVariableNameProvider variableNameProvider, Modifier modifier)
		{
			return typeof(SerializeBodyProvider).Assembly.GetTypes()
				.Where(x => x.GetInterfaces().Contains(typeof(ICodeProvider)))
				.Where(x =>
				{
					var attribute = x.GetCustomAttribute<CodeProviderAttribute>(true);
					return attribute != null && attribute.Modifier == modifier;
				})
				.Select(x => x.GetConstructors().First().Invoke(new object[] { variableNameProvider }))
				.Cast<ICodeProvider>()
				.First();
		}
	}

	public interface ICodeProvider
	{
		BlockSyntax SerializeSize(IdentifierNameSyntax size);
		BlockSyntax Serialize(MemberAccessExpressionSyntax member, IdentifierNameSyntax array, IdentifierNameSyntax position);
		BlockSyntax Deserialize(bool isValueType, IdentifierNameSyntax type, MemberAccessExpressionSyntax member, IdentifierNameSyntax array, IdentifierNameSyntax position);
	}

	[CodeProvider(Modifier.Required)]
	public class RequiredProvider : ICodeProvider
	{
		private readonly IVariableNameProvider _variableNameProvider;

		public RequiredProvider(IVariableNameProvider variableNameProvider)
		{
			_variableNameProvider = variableNameProvider;
		}

		public BlockSyntax SerializeSize(IdentifierNameSyntax size)
		{
			// size++;
			return Block
			(
				ExpressionStatement
				(
					PostfixUnaryExpression
					(
						SyntaxKind.PostIncrementExpression,
						size
					)
				)
			);
		}

		public BlockSyntax Serialize(MemberAccessExpressionSyntax member, IdentifierNameSyntax array,
			IdentifierNameSyntax position)
		{
			// array[position++] = instance.value;
			return Block
			(
				ExpressionStatement
				(
					AssignmentExpression
					(
						SyntaxKind.SimpleAssignmentExpression,
						ElementAccessExpression(array)
							.WithArgumentList
							(
								BracketedArgumentList
								(
									SingletonSeparatedList
									(
										Argument
										(
											PostfixUnaryExpression
											(
												SyntaxKind.PostIncrementExpression,
												position
											)
										)
									)
								)
							),
						member
					)
				)
			);
		}

		public BlockSyntax Deserialize(bool isValueType, IdentifierNameSyntax type, MemberAccessExpressionSyntax member,
			IdentifierNameSyntax array, IdentifierNameSyntax position)
		{
			var returnFalse =
				Block(ReturnStatement(LiteralExpression(SyntaxKind.FalseLiteralExpression)));

			var isTVariableName = _variableNameProvider.New();
			var objName = _variableNameProvider.New();

			var code = Block
			(
				// if (array.Length <= position)
				IfStatement
				(
					BinaryExpression
					(
						SyntaxKind.LessThanOrEqualExpression,
						MemberAccessExpression
						(
							SyntaxKind.SimpleMemberAccessExpression,
							array,
							IdentifierName("Length")
						),
						position
					),
					// { return false; }
					returnFalse
				),

				// object obj = array[position++];
				LocalDeclarationStatement
				(
					VariableDeclaration(PredefinedType(Token(SyntaxKind.ObjectKeyword)))
						.WithVariables(SingletonSeparatedList(VariableDeclarator(objName)
							.WithInitializer(EqualsValueClause(ElementAccessExpression(array)
								.WithArgumentList(BracketedArgumentList(
									SingletonSeparatedList(
										Argument(
											PostfixUnaryExpression(SyntaxKind.PostIncrementExpression,
												position)))))))))
				)
			);

			if (isValueType)
			{
				code = code.AddStatements(
					// value types only

					// if (!(obj is T theRightValue)) return false;
					IfStatement
					(
						PrefixUnaryExpression
						(
							SyntaxKind.LogicalNotExpression,
							ParenthesizedExpression
							(
								IsPatternExpression
								(
									IdentifierName(objName),
									DeclarationPattern
									(
										type,
										SingleVariableDesignation(Identifier(isTVariableName))
									)
								)
							)
						),
						Block
						(
							SingletonList<StatementSyntax>(
								node: ReturnStatement(LiteralExpression(SyntaxKind.FalseLiteralExpression)))
						)
					),

					// instance.Whatever = theRightValue;
					ExpressionStatement
					(
						AssignmentExpression
						(
							SyntaxKind.SimpleAssignmentExpression,
							member,
							IdentifierName(isTVariableName)
						)
					)
				);
			}
			else
			{
				code = code.AddStatements(
					// value types only

					// if (obj == null)
					IfStatement
						(
							BinaryExpression
							(
								SyntaxKind.EqualsExpression,
								IdentifierName(objName),
								LiteralExpression(SyntaxKind.NullLiteralExpression)
							),
							Block
							(
								SingletonList<StatementSyntax>
								(
									ExpressionStatement
									(
										AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, member,
											LiteralExpression(SyntaxKind.NullLiteralExpression))
									)
								)
							)
						)
						.WithElse
						(
							ElseClause
							(
								IfStatement
								(
									IsPatternExpression
									(
										IdentifierName(objName),
										DeclarationPattern
										(
											type,
											SingleVariableDesignation(Identifier(isTVariableName))
										)
									),
									Block
									(
										SingletonList<StatementSyntax>
										(
											ExpressionStatement
											(
												AssignmentExpression
												(
													SyntaxKind.SimpleAssignmentExpression,
													member,
													IdentifierName(isTVariableName)
												)
											)
										)
									)
								)
									.WithElse
									(
										ElseClause
										(
											Block
											(
												SyntaxFactory.SingletonList<StatementSyntax>(SyntaxFactory.ReturnStatement(SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression)))
											)
										)
									)
							)
						)
				);
			}

			return code;
		}
	}

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public sealed class CodeProviderAttribute : Attribute
	{
		public Modifier Modifier { get; }

		public CodeProviderAttribute(Modifier modifier) => Modifier = modifier;
	}
}