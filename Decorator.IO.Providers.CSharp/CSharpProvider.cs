using Decorator.IO.Core;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using System.IO;
using System.Text;

namespace Decorator.IO.Providers.CSharp
{
	public class CSharpProvider : IProvider
	{
		public void Provide(Stream outputStream, DecoratorFile file)
		{
			var result =
				SyntaxFactory.SyntaxTree
				(
					CSharpSyntaxTree.ParseText(@"public class Ree
{
	public void AhahahAHAH() {
		throw new Exception         (""oh n o "");
	}
}", CSharpParseOptions.Default)
	.GetRoot()
				)
				.GetCompilationUnitRoot()
				.NormalizeWhitespace()
				.ToFullString();

			using (var sw = new StreamWriter(outputStream, Encoding.UTF8, 0x1000, true))
			{
				sw.Write(result);
			}
		}
	}
}