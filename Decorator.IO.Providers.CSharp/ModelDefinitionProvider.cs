using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Decorator.IO.Core.Tokens;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Decorator.IO.Providers.CSharp
{
	public class ModelDefinitionProvider : IRoslynCodeProvider
	{
		private readonly Model _model;

		public ModelDefinitionProvider(Model model)
		{
			_model = model;
		}

		public MemberDeclarationSyntax Provide()
		{
			var options = new CSharpParseOptions(LanguageVersion.Latest);
			var properties = new ModelFlattener(_model).FlattenToFields().Select(x => GeneratePropertySyntax(x));
			var methods = new[]
			{
				MethodDeclaration(
						ArrayType(PredefinedType(Token(SyntaxKind.ObjectKeyword))).WithRankSpecifiers(
							SingletonList<ArrayRankSpecifierSyntax>(
								ArrayRankSpecifier(
									SingletonSeparatedList<ExpressionSyntax>(OmittedArraySizeExpression())))),
						"Serialize")
					.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
					.WithExpressionBody(ArrowExpressionClause
						(
							InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
									IdentifierName("DecoratorIO"), IdentifierName($"Serialize{_model.Identifier}")))
								.WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(ThisExpression()))))
						)
					)
					.WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),

				MethodDeclaration(PredefinedType(Token(SyntaxKind.BoolKeyword)), "TryDeserialize")
					.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword)))
					.WithParameterList(ParameterList(SeparatedList<ParameterSyntax>(new SyntaxNodeOrToken[]
					{
						// object[] data
						Parameter(Identifier("data"))
							.WithType(ArrayType(PredefinedType(Token(SyntaxKind.ObjectKeyword)))
								.WithRankSpecifiers(SingletonList<ArrayRankSpecifierSyntax>(
									ArrayRankSpecifier(
										SingletonSeparatedList<ExpressionSyntax>(OmittedArraySizeExpression()))))),

						Token(SyntaxKind.CommaToken),

						// out (model name) instance
						Parameter(Identifier("instance"))
							.WithModifiers(TokenList(Token(SyntaxKind.OutKeyword)))
							.WithType(IdentifierName(_model.Identifier))
					})))
					.WithExpressionBody(ArrowExpressionClause(InvocationExpression(MemberAccessExpression
						(
							SyntaxKind.SimpleMemberAccessExpression,
							IdentifierName("DecoratorIO"),
							IdentifierName("TryDeserialize" + _model.Identifier)
						))
						.WithArgumentList(ArgumentList(SeparatedList<ArgumentSyntax>(new SyntaxNodeOrToken[]
						{
							Argument(IdentifierName("data")), Token(SyntaxKind.CommaToken),
							Argument(IdentifierName("instance")).WithRefKindKeyword(Token(SyntaxKind.OutKeyword))
						})))))
					.WithSemicolonToken ( Token(SyntaxKind.SemicolonToken) ),

				
				CSharpSyntaxTree.ParseText(@"
public override string ToString() => $""" + properties.Select(x => $"{x.Identifier}: {{this.{x.Identifier}}}").Aggregate((l, r) => $"{l}, {r}") + @""";", options).GetCompilationUnitRoot()
				.Members.First()
		};

			

			return ClassDeclaration(_model.Identifier)
				.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.SealedKeyword)))
				.WithBaseList(BaseList(
					SingletonSeparatedList<BaseTypeSyntax>(SimpleBaseType(IdentifierName("I" + _model.Identifier)))))
				.WithMembers(List(properties.Cast<MemberDeclarationSyntax>()
					.Concat(methods.Cast<MemberDeclarationSyntax>()).ToArray()));
		}

		public PropertyDeclarationSyntax GeneratePropertySyntax(Field field)
			=> PropertyDeclaration(IdentifierName(field.Type.Identifier), field.Identifier)
				.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
				.WithAccessorList(AccessorList(List(new[]
					{
						AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
							.WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),

						AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
							.WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
					}
				)));
	}
}