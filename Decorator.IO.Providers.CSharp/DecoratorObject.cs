using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;

namespace Decorator.IO.Providers.CSharp
{
	public static class DecoratorObject
	{
		public static IEnumerable<MemberDeclarationSyntax> Create()
			=> $@"public interface {Config.InterfaceDecoratorObject}
{{
	object[] Serialize();
}}"
			.AsCompilationUnitSyntax()
			.AsMemberDeclarationSyntaxes();
	}
}