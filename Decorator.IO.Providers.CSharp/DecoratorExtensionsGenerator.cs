using Decorator.IO.Core;
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
	{decoratorClassNames.Select(x => $@"public static object[] SerializeAs{x}(this I{x} obj)
{{
	return DecoratorObject.Serialize(obj);
}}")
				.Aggregate((a, b) => $"{a}\n{b}")}
}}"
			.AsCompilationUnitSyntax()
			.AsMemberDeclarationSyntaxes();
	}
}