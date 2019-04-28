using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Linq;

namespace Decorator.IO.Providers.CSharp
{
	public static class DecoratorObject
	{
		public static MemberDeclarationSyntax Create()
			=> $@"public interface {Config.DecoratorName}
{{
	object[] Serialize();
}}".AsCompilationUnitSyntax()
			.ChildNodes()
			.Cast<MemberDeclarationSyntax>()
			.First();
	}
}