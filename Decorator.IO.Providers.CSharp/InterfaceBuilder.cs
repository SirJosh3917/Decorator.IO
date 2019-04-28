using Decorator.IO.Core;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.Providers.CSharp
{
	public static class InterfaceBuilder
	{
		public static IEnumerable<MemberDeclarationSyntax> BuildInterface(DecoratorClass decoratorClass)
			=> $@"public interface {Config.InterfaceName(decoratorClass.Name)} {decoratorClass.Parents.Select(x => x.Name).InterfaceInherits()}
{{
	{decoratorClass.UniqueFields().ToPropertyStrings(false)}
}}"
			.AsCompilationUnitSyntax()
			.AsMemberDeclarationSyntaxes();
	}
}