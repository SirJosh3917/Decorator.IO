using Decorator.IO.Core;
using Humanizer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decorator.IO.Providers.CSharp
{
	public class CSharpCasing
	{
		// i don't like changing stuff by reference
		// but i'm lazy lol

		public void Apply(DecoratorFile @in)
		{
			@in.Namespace = @in.Namespace.Split('.')
				.Select(x => x.Pascalize())
				.Aggregate((a, b) => $"{a}.{b}");

			foreach (var x in @in.Classes)
			{
				x.Name = x.Name.Pascalize();
			}

			foreach(var i in @in.Classes.SelectMany(x => x.Fields))
			{
				i.Name = i.Name.Pascalize();
			}
		}
	}

	public class CSharpProvider : IProvider
	{
		public async Task Provide(Stream outputStream, DecoratorFile file)
		{
			new CSharpCasing()
				.Apply(file);

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
									.Select(decoratorClass => new ClassBuilder().BuildInterface(decoratorClass))
									.Select(compilationUnit => compilationUnit.ChildNodes().First())
									.Cast<MemberDeclarationSyntax>()
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