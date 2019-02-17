using Decorator.IO.Core.Tokens;
using Decorator.IO.Providers.Core;

using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.Providers.CSharp.Generators
{
	public class FieldToPropertyGenerator : IGenerator
	{
		private readonly Field _field;
		private readonly bool _addPublic;

		public FieldToPropertyGenerator(Field field, bool addPublic)
		{
			_field = field;
			_addPublic = addPublic;
		}

		public IEnumerable<GeneratorItem> Generate()
		{
			var modifiers = new StringBuilder();

			if (_addPublic)
			{
				modifiers.Append("public ");
			}

			modifiers.Append(_field.Type.Identifier);

			modifiers.Append(" { get; set; }");

			yield return modifiers.ToString();
		}
	}
}