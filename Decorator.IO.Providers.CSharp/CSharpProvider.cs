using Decorator.IO.Core.Tokens;
using Decorator.IO.Providers.Core;

using Humanizer;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decorator.IO.Providers.CSharp
{
	public class CSharpProvider : LanguageProvider
	{
		public override string ModifyStringCasing(string str) => str.Pascalize();

		public override IEnumerable<StringBuilder> GenerateFrom(Namespace dioNamespace)
		{
			foreach (var line in SyntaxFactory.CompilationUnit()
				.WithMembers
				(
					SyntaxFactory.SingletonList<MemberDeclarationSyntax>(
						SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName(dioNamespace.Name))
							.WithMembers(SyntaxFactory.List(
									new[]
										{
											new IModelNonGenericInterface().Provide(),
											new IModelProvider().Provide(),
										}
										.Concat(dioNamespace.Models.Select(x =>
											new ModelInterfaceProvider(x).Provide()))
										.Concat(dioNamespace.Models.Select(x =>
											new ModelDefinitionProvider(x).Provide()))
										.Concat(new []
										{
											new DecoratorIOProvider(dioNamespace.Models, new DefaultVariableNameProvider()).Provide()
										})
										.ToArray()
								)
							)
					)

				).NormalizeWhitespace().ToFullString().Replace("\r", "").Split('\n'))
			{
				yield return new StringBuilder(line);
			}
		}
	}
}