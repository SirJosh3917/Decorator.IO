using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Decorator.IO.Core.Tokens;
using Decorator.IO.Providers.Core;

namespace Decorator.IO.Providers.CSharp.Generators
{
	public class SerializeCodeGenerator : IGenerator
	{
		private readonly Model _model;
		private readonly Field[] _fields;

		public SerializeCodeGenerator(Model model)
		{
			_model = model;
			_fields = new ModelFlattener(_model).FlattenToFields()
				.OrderBy(f => f.Position)
				.ToArray();
		}

		public IEnumerable<GeneratorItem> Generate()
		{
			yield return "int size = 0;";

			foreach (var field in _fields)
			{
				switch (field.Modifier)
				{
					case Modifier.Required:
					{
						yield return "size++;";
					}
						break;

					default: throw new NotSupportedException(field.Modifier.ToString());
				}
			}

			yield return "object[] array = new object[size];";

			int position = 0;
			yield return "int position = 0;";

			Field lastField = default;
			foreach (var field in _fields)
			{
				if (lastField != null)
				{
					var space = field.Position - lastField.Position;

					var strb = new StringBuilder("position += ");
					strb.Append(space.ToString());
					strb.Append(";");
					yield return strb;
				}

				switch (field.Modifier)
				{
					case Modifier.Required:
					{
						var strb = new StringBuilder("array[position] = instance.");
						strb.Append(field.Identifier);
						strb.Append(";");

						yield return strb;
					}
					break;

					default: throw new NotSupportedException(field.Modifier.ToString());
				}

				lastField = field;
			}

			yield return "return array;";
		}
	}
}
