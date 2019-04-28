using Decorator.IO.Core;
using System;
using System.Collections.Generic;

namespace Decorator.IO.Providers.CSharp
{
	public class AssignGenerator
	{
		private readonly DecoratorFile _context;

		public AssignGenerator(DecoratorFile context)
		{
			_context = context;
			AssignTable[Modifier.Optional] = AssignTable[Modifier.Required];
		}

		public Dictionary<Modifier, Func<DecoratorField, string, string, string>> AssignTable = new Dictionary<Modifier, Func<DecoratorField, string, string, string>>
		{
			[Modifier.Required] = (decoratorField, objectContext, counterName) => $@"{Config.ArrayName}[{counterName}] = {objectContext}{decoratorField.Name};
{counterName}++;",
			// [Modifier.Optional],
		};

		public string Assign(DecoratorField decoratorField, string objectContext, string counterName)
			=> AssignTable[decoratorField.Modifier](decoratorField, objectContext, counterName);
	}
}