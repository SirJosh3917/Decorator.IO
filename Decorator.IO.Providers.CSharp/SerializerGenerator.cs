﻿using Decorator.IO.Core;
using System;
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
			var generator = new AssignGenerator(_context);

			if (decoratorClass.Fields.Length == 0)
			{
				return $@"return new object[0];";
			}

			return $@"object[] {Config.ArrayName} = new object[{GenerateSize(decoratorClass)}];
int counter = {decoratorClass.Fields.OrderBy(x => x.Index).First().Index};
{
				decoratorClass.Fields.Select(x => generator.Assign(x, $"{Config.ObjectName}.", "counter"))
					.Select(x => $"{Config.ArrayName}[counter] = {x}")
					.NewlineAggregate()
}
return array;";
		}

		public string GenerateSize(DecoratorClass decoratorClass)
		{
			var generator = new SizeGenerator(_context);

			return $@"(0 {decoratorClass.Fields.Select(x => generator.GenerateSize(x, $"{Config.ObjectName}."))
				.Aggregate((a, b) => a + b)})";
		}
	}
}