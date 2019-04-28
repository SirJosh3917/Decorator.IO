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
	public static object[] {Config.SerializeName}(this {Config.InterfaceDecoratorObject} unsupportedDecoratorObject)
	{{
		// TODO: switch statement on the item to check the different types
		throw new System.NotSupportedException(""Please attempt to look for the correct overload"");
	}}

	{classes.Select(WriteFunctionCode).NewlineAggregate()}
}}"
			.AsCompilationUnitSyntax()
			.AsMemberDeclarationSyntaxes();

		public static string WriteFunctionCode(DecoratorClass decoratorClass)
		{
			return $@"public static {Config.InterfaceName(decoratorClass.Name)} {Config.DeserializeAsName(decoratorClass.Name)}(object[] array)
{{
	return default;
}}

public static object[] {Config.SerializeAsName(decoratorClass.Name)}({Config.InterfaceName(decoratorClass.Name)} obj)
{{
	return default;
}}
";
		}
	}
}