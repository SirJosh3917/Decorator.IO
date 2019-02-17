using Decorator.IO.Core.Tokens;
using Decorator.IO.Providers.Core;

using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.Providers.CSharp.Generators
{
	public class PropertyGenerator : IGenerator
	{
		private readonly Field _field;
		private readonly bool _addPublic;

		public PropertyGenerator(Field field, bool addPublic)
		{
			_field = field;
			_addPublic = addPublic;
		}

		public IEnumerable<GeneratorItem> Generate()
		{
			var property = new StringBuilder();

			if (_addPublic)
			{
				property.Append("public ");
			}

			property.Append(_field.Type.Identifier);

			property.Append(" ");

			property.Append(_field.Identifier);

			property.Append(" { get; set; }");

			yield return property.ToString();
		}
	}
}