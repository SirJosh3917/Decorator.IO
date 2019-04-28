using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Decorator.IO.Providers.CSharp
{
	public static class Helpers
	{
		public static CompilationUnitSyntax AsCompilationUnitSyntax(this string str)
			=> CSharpSyntaxTree.ParseText(str, CSharpParseOptions.Default).GetCompilationUnitRoot();
	}
}