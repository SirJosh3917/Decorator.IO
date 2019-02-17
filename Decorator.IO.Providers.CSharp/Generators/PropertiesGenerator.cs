using Decorator.IO.Core.Tokens;
using Decorator.IO.Providers.Core;

using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.Providers.CSharp.Generators
{
	public class PropertiesGenerator : IGenerator
	{
		private readonly Field[] _fields;
		private readonly bool _addPublic;

		public PropertiesGenerator(Field[] fields, bool addPublic)
		{
			_fields = fields;
			_addPublic = addPublic;
		}

		public IEnumerable<GeneratorItem> Generate()
		{
			return _fields
				.SelectMany
				(
					field => new PropertyGenerator(field, _addPublic)
							.Generate()
				);
		}
	}
}