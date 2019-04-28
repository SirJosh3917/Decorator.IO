using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Linq;

namespace Decorator.IO.Providers.CSharp
{
	public static class DecoratorObject
	{
		public static MemberDeclarationSyntax Create()
			=> CSharpSyntaxTree.ParseText($@"public interface {Config.DecoratorName}
{{
	object[] Serialize();
}}", CSharpParseOptions.Default)
				.GetCompilationUnitRoot()
			.ChildNodes()
			.Cast<MemberDeclarationSyntax>()
			.First();
	}
}