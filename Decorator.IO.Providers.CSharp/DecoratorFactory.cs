using Decorator.IO.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decorator.IO.Providers.CSharp
{
	public static class DecoratorFactory
	{
		public static IEnumerable<MemberDeclarationSyntax> BuildClass(DecoratorClass[] classes)
		=> $@"public static class DecoratorFactory
{{
	public static object[] Serialize(this {Config.DecoratorName} unsupportedDecoratorObject)
	{{
		throw new NotSupportedException(""Please attempt to look for the correct overload"");
	}}

	{classes.Select(MakeSerializeFunction).NewlineAggregate()}
}}"
			.AsCompilationUnitSyntax()
			.AsMemberDeclarationSyntaxes();

		public static string MakeSerializeFunction(DecoratorClass decoratorClass)
		{
			return "// " + decoratorClass.Name;
		}
	}
}
