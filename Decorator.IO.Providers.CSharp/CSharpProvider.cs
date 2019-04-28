using Decorator.IO.Core;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decorator.IO.Providers.CSharp
{
	public class CSharpProvider : IProvider
	{
		public async Task Provide(Stream outputStream, DecoratorFile file)
		{
			file.ApplyCSharpCasing();

			var result = SyntaxFactory.CompilationUnit()
				.WithMembers
				(
					SyntaxFactory.SingletonList<MemberDeclarationSyntax>
					(
						SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName(file.Namespace))
						.WithMembers
						(
							new SyntaxList<MemberDeclarationSyntax>
							(
								file
									.Classes
									.Select(decoratorClass => new InterfaceBuilder().BuildInterface(decoratorClass))
									.SelectMany(compilationUnit => compilationUnit.AsMemberDeclarationSyntaxes())
								.Concat
								(
									file.Classes
										.Select(decoratorClass => new ClassBuilder().BuildClass(decoratorClass))
										.SelectMany(compilationUnit => compilationUnit.AsMemberDeclarationSyntaxes())
								)
								.Append
								(
									DecoratorObject.Create()
								)
							)
						)
					)
				)
				.NormalizeWhitespace()
				.ToFullString();

			using (var sw = new StreamWriter(outputStream, Encoding.UTF8, 0x1000, true))
			{
				sw.Write(result);
			}
		}
	}
}