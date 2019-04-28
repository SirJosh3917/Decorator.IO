using Decorator.IO.Core;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.Providers.CSharp
{
	public class DecoratorFactory
	{
		private readonly DecoratorFile _context;

		public DecoratorFactory(DecoratorFile context)
		{
			_context = context;
		}

		public IEnumerable<MemberDeclarationSyntax> BuildClass(DecoratorClass[] classes)
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

		public string WriteFunctionCode(DecoratorClass decoratorClass)
		{
			var serializerCode = new SerializerCode(_context);
			var deserializerCode = new DeserializerCode(_context);

			return $@"public static {Config.InterfaceName(decoratorClass.Name)} {Config.DeserializeAsName(decoratorClass.Name)}(object[] {Config.ArrayName})
{{
	{deserializerCode.Generate(decoratorClass)}
}}

public static object[] {Config.SerializeAsName(decoratorClass.Name)}({Config.InterfaceName(decoratorClass.Name)} {Config.ObjectName})
{{
	{serializerCode.Generate(decoratorClass)}
}}
";
		}
	}
}