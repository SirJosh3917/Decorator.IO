using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Decorator.IO.Providers.CSharp
{
	internal interface IRoslynCodeProvider
	{
		MemberDeclarationSyntax Provide();
	}
}