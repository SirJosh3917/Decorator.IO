using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Decorator.IO.Core.Tokens;
using Decorator.IO.Providers.Core;

namespace Decorator.IO.Providers.CSharp.Generators
{
	public class DeserializeCodeGenerator : IGenerator
	{
		private readonly Model _model;
		private readonly Field[] _fields;

		public DeserializeCodeGenerator(Model model)
		{
			_model = model;
			_fields = new ModelFlattener(_model).FlattenToFields()
				.OrderBy(f => f.Position)
				.ToArray();
		}

		public IEnumerable<GeneratorItem> Generate()
		{
			var strb = new StringBuilder("instance = new ");
			strb.Append(_model.Identifier);
			strb.Append("();");
			yield return strb;

			int varOn = 0;

			yield return "int position = 0;";

			Field previousField = null;
			foreach (var field in _fields)
			{
				if (previousField != null)
				{
					var increment = field.Position - previousField.Position;

					var strb2 = new StringBuilder("position += ");
					strb2.Append(increment.ToString());
					strb2.Append(";");
					yield return strb2;
				}

				var varName = "_" + varOn++.ToString();
				var castName = "_" + varOn++.ToString();

				switch (field.Modifier)
				{
					case Modifier.Required:
					{
						yield return "if (data.Length <= position) return false;";
						yield return $"var {varName} = data[position];";

						if (field.Type.IsValueType)
							yield return $"if ({varName} == null) return false;";

						yield return $"if (!({varName} is {field.Type.Identifier} {castName})) return false;";
						yield return $"instance.{field.Identifier} = {castName};";
					}
					break;

					default: throw new NotSupportedException();
				}

				previousField = field;
			}

			yield return "return true;";
		}
	}
}
