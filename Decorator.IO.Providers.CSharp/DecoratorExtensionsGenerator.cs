using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.Providers.CSharp
{
	public static class DecoratorExtensionsGenerator
	{
		public static IEnumerable<MemberDeclarationSyntax> Create(IEnumerable<string> decoratorClassNames)
			=> $@"public static class DecoratorExtensionsGenerator
{{
	{decoratorClassNames.Select(x => $@"public static object[] {Config.SerializeAsName(x)}(this {Config.InterfaceName(x)} {Config.ObjectName})
{{
	return {Config.DecoratorFactory}.{Config.SerializeAsName(x)}({Config.ObjectName});
}}

public static {Config.InterfaceName(x)} {Config.DeserializeAsName(x)}(this object[] {Config.ArrayName})
{{
	return {Config.DecoratorFactory}.{Config.DeserializeAsName(x)}({Config.ArrayName});
}}")
				.NewlineAggregate()}
}}"
			.AsCompilationUnitSyntax()
			.AsMemberDeclarationSyntaxes();
	}
}