using Decorator.IO.Core;

using System;
using System.Collections.Generic;

namespace Decorator.IO.Providers.CSharp
{
	public class SizeGenerator
	{
		private readonly DecoratorFile _context;

		public SizeGenerator(DecoratorFile context)
		{
			_context = context;
		}

		public Dictionary<Modifier, Func<DecoratorField, string, string>> SizeTable = new Dictionary<Modifier, Func<DecoratorField, string, string>>
		{
			[Modifier.Required] = (_, __) => "+ 1",
			[Modifier.Optional] = (_, __) => "+ 1",
		};

		public string GenerateSize(DecoratorField field, string objectContext)
		{
			return SizeTable[field.Modifier](field, objectContext);
		}
	}
}