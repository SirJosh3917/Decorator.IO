using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Decorator.IO.Providers.CSharp
{
	public class DecoratorIOProvider : IRoslynCodeProvider
	{
		private readonly MemberDeclarationSyntax[] _methods;

		public DecoratorIOProvider(params MemberDeclarationSyntax[] methods)
		{
			_methods = methods;
		}

		public MemberDeclarationSyntax Provide()
		{
			return ClassDeclaration("DecoratorIO")
				.WithModifiers(TokenList(new[] {Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword)}))
				.WithMembers(List(_methods));
		}
	}
}
