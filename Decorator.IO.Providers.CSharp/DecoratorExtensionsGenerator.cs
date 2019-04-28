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
	return {Config.DecoratorFactory}.{Config.InterfaceSerializeName}(obj);
}}")
				.NewlineAggregate()}
}}"
			.AsCompilationUnitSyntax()
			.AsMemberDeclarationSyntaxes();
	}
}