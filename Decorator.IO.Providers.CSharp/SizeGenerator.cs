using Decorator.IO.Core;

using System;
using System.Collections.Generic;

namespace Decorator.IO.Providers.CSharp
{
	public class SizeGenerator
	{
		private readonly DecoratorFile _context;
		private readonly DecoratorClass _classContext;

		public SizeGenerator(DecoratorFile context, DecoratorClass classContext)
		{
			_context = context;
			_classContext = classContext;
			SizeTable[Modifier.Required] = SizeRequired;
			SizeTable[Modifier.Optional] = SizeTable[Modifier.Required];
		}

		public Dictionary<Modifier, Func<DecoratorField, string, string>> SizeTable = new Dictionary<Modifier, Func<DecoratorField, string, string>>
		{
		};

		public string SizeRequired(DecoratorField decoratorField, string objectContext)
		{
			var fields = _classContext.AllFieldsOf();

			var index = Array.IndexOf(fields, decoratorField);

			// if there's none before us just return our index
			if (index - 1 < 0)
			{
				return $"+ 1 + {decoratorField.Index}";
			}

			// otherwise increment counter by the amount we need to advance
			int needAdvance = fields[index].Index - fields[index - 1].Index;

			return $"+ {needAdvance}";
		}

		public string GenerateSize(DecoratorField field, string objectContext)
		{
			return SizeTable[field.Modifier](field, objectContext);
		}
	}
}