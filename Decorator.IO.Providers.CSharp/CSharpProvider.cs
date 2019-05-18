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

			var decoratorFactory = new DecoratorFactory(file);
			var messageDeserializationSystem = new MessageDeserializationSystem(new NameGenerator());

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
								file.Classes.SelectMany(InterfaceBuilder.BuildInterface)
								.Concat(file.Classes.SelectMany(ClassBuilder.BuildClass))
								.Concat(DecoratorExtensionsGenerator.Create(file.Classes.Select(x => x.Name)))
								.Concat(DecoratorObject.Create())
								.Concat(decoratorFactory.BuildClass(file.Classes))
								.Concat(messageDeserializationSystem.Create(file.Classes))
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