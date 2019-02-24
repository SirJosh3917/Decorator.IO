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
	public class ModelInterfaceProvider : IRoslynCodeProvider
	{
		private readonly Model _model;

		public ModelInterfaceProvider(Model model)
		{
			_model = model;
		}

		public static bool ModelParentsDontHaveField(Field thisField, Model model)
		{
			return !model.Parents
					// if the parent has any field with the same identifier
				.Any(parent => parent.Model.Fields.Any(parentField => parentField.Identifier == thisField.Identifier)

								 // or the parent has fields up above itself
				                 || ModelParentsDontHaveField(thisField, parent.Model));
		}

		public MemberDeclarationSyntax Provide()
		{
			var properties = _model.Fields.Where(field => ModelParentsDontHaveField(field, _model)).Select(x => GeneratePropertySyntax(x));

			return InterfaceDeclaration($"I{_model.Identifier}")
				.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
				.WithBaseList(BaseList(SeparatedList<BaseTypeSyntax>(GetBaseList(_model))))
				.WithMembers(List(properties.Cast<MemberDeclarationSyntax>().ToArray()));
		}

		public static SyntaxNodeOrToken[] GetBaseList(Model model)
		{
			return new SyntaxNodeOrToken[]
			{
				// IModel<I(model name)>
				SimpleBaseType(GenericName("IModel").WithTypeArgumentList(TypeArgumentList(SingletonSeparatedList<TypeSyntax>(IdentifierName($"I{model.Identifier}")))))
			}
				.Concat(model.Parents.SelectMany(x => new SyntaxNodeOrToken[]
				{
					Token(SyntaxKind.CommaToken),
					SimpleBaseType(IdentifierName($"I{x.Model.Identifier}"))
				}))
					.ToArray();
		}

		public static PropertyDeclarationSyntax GeneratePropertySyntax(Field field)
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
