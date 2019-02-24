using Decorator.IO.Core.Tokens;
using Decorator.IO.Providers.Core;

using Humanizer;

using Microsoft.CodeAnalysis;

using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
					SyntaxFactory.List<MemberDeclarationSyntax>
					(
						new[]
						{
							new ModelNonGenericInterface().Provide(),
							new ModelInterface().Provide()
						}
					)
				).NormalizeWhitespace().ToFullString().Split('\n'))
			{
				yield return new StringBuilder(line);
			}
		}
	}
}