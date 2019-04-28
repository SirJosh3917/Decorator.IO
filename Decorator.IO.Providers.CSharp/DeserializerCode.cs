using System;
using System.Linq;
using Decorator.IO.Core;

namespace Decorator.IO.Providers.CSharp
{
	internal class DeserializerCode
	{
		private DecoratorFile _context;

		public DeserializerCode(DecoratorFile context) => _context = context;

		public string Generate(DecoratorClass decoratorClass)
		{

			var fields = decoratorClass.AllFieldsOf();

			if (fields.Length == 0)
			{
				return $"return new {decoratorClass.Name}();";
			}

			var desGen = new DeserializerGenerator(_context, decoratorClass, new NameGenerator());

			return $@"{decoratorClass.Name} {Config.ObjectName} = new {decoratorClass.Name}();
int index = 0;
{
				fields.Select(x => desGen.GenerateCode(x, Config.ObjectName, "index"))
					.Select(x => $@"if (index >= {Config.ArrayName}.Length)
{{
	throw new System.Exception(""not a big enough array"");
}}

{x}")
					.NewlineAggregate()
}
return {Config.ObjectName};";
		}
	}
}