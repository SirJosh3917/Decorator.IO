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
	{decoratorClassNames.Select(x => $@"public static object[] {Config.SerializeAsName(x)}(this I{x} obj)
{{
	return {Config.DecoratorFactory}.{Config.SerializeAsName(x)}(obj);
}}

public static I{x} {Config.DeserializeAsName(x)}(this object[] array)
{{
	return {Config.DecoratorFactory}.{Config.DeserializeAsName(x)}(array);
}}")
				.NewlineAggregate()}
}}"
			.AsCompilationUnitSyntax()
			.AsMemberDeclarationSyntaxes();
	}
}