﻿using System;
using System.Linq;
using Decorator.IO.Core;

namespace Decorator.IO.Providers.CSharp
{
	internal class DeserializerCode
	{
		private DecoratorFile _context;

		public DeserializerCode(DecoratorFile context) => _context = context;

		public string Generate(DecoratorClass decoratorClass, bool returnFalse = false)
		{

			var fields = decoratorClass.AllFieldsOf();

			if (fields.Length == 0)
			{
				return returnFalse
					? $@"{Config.ObjectName} = new {decoratorClass.Name}();
return true;"
					: $"return new {decoratorClass.Name}();";
			}

			var desGen = new DeserializerGenerator(_context, decoratorClass, new NameGenerator());
			desGen.ReturnFalse = returnFalse;

			return $@"{(returnFalse ? "" : decoratorClass.Name + " ")}{Config.ObjectName} = new {decoratorClass.Name}();
int index = 0;
{
				fields.Select(x => desGen.GenerateCode(x, Config.ObjectName, "index"))
					.Select(x => $@"if (index >= {Config.ArrayName}.Length)
{{
	{desGen.Fail("Array is not big enough.")}
}}

{x}")
					.NewlineAggregate()
}" + (returnFalse
? "return true;"
: $"return {Config.ObjectName};");
		}
	}
}