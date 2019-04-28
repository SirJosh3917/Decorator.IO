using Decorator.IO.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.Providers.CSharp
{
	public class SerializerCode
	{
		private readonly DecoratorFile _context;

		public SerializerCode(DecoratorFile context)
		{
			_context = context;
		}

		// TODO: having to do decoratorClass.AllFieldsOf is *very* messy and i don't like it not one bit

		public string Generate(DecoratorClass decoratorClass)
		{
			if (decoratorClass.Fields.Length == 0)
			{
				return $@"return new object[0];";
			}

			var fields = decoratorClass.AllFieldsOf();

			var generator = new AssignGenerator(_context, decoratorClass);

			return $@"object[] {Config.ArrayName} = new object[{GenerateSize(decoratorClass)}];
int counter = {fields.OrderBy(x => x.Index).First().Index};
{
				fields.Select(x => generator.Assign(x, $"{Config.ObjectName}.", "counter"))
					.NewlineAggregate()
}
return array;";
		}

		public string GenerateSize(DecoratorClass decoratorClass)
		{
			var generator = new SizeGenerator(_context, decoratorClass);
			var fields = decoratorClass.AllFieldsOf();

			return $@"(0 {fields.Select(x => generator.GenerateSize(x, $"{Config.ObjectName}."))
				.Aggregate((a, b) => a + b)})";
		}
	}
}