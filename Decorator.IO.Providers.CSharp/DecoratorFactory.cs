using Decorator.IO.Core;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.Providers.CSharp
{
	public static class DecoratorFactory
	{
		public static IEnumerable<MemberDeclarationSyntax> BuildClass(DecoratorClass[] classes)
		=> $@"public static class {Config.DecoratorFactory}
{{
	public static object[] {Config.InterfaceSerializeName}(this {Config.InterfaceDecoratorObject} unsupportedDecoratorObject)
	{{
		throw new System.NotSupportedException(""Please attempt to look for the correct overload"");
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