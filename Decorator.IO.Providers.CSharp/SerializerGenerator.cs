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

		public string Generate(DecoratorClass decoratorClass)
		{
			if (decoratorClass.Fields.Length == 0)
			{
				return $@"return new object[0];";
			}

			var generator = new AssignGenerator(_context, decoratorClass);

			return $@"object[] {Config.ArrayName} = new object[{GenerateSize(decoratorClass)}];
int counter = {decoratorClass.Fields.OrderBy(x => x.Index).First().Index};
{
				decoratorClass.Fields.Select(x => generator.Assign(x, $"{Config.ObjectName}.", "counter"))
					.NewlineAggregate()
}
return array;";
		}

		public string GenerateSize(DecoratorClass decoratorClass)
		{
			var generator = new SizeGenerator(_context, decoratorClass);

			return $@"(0 {decoratorClass.Fields.Select(x => generator.GenerateSize(x, $"{Config.ObjectName}."))
				.Aggregate((a, b) => a + b)})";
		}
	}
}