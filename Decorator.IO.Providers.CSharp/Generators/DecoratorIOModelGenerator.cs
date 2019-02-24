using Decorator.IO.Core.Tokens;
using Decorator.IO.Providers.Core;

using System.Collections.Generic;
using System.Text;
using Decorator.IO.Providers.Core.Processes;

namespace Decorator.IO.Providers.CSharp.Generators
{
	public class DecoratorIOModelGenerator : IGenerator
	{
		private readonly Model _model;

		public DecoratorIOModelGenerator(Model model) => _model = model;

		public IEnumerable<GeneratorItem> Generate()
		{
			var serialize = new StringBuilder("public static object[] Serialize");
			serialize.Append(_model.Identifier);
			serialize.Append("(");
			serialize.Append(_model.Identifier);
			serialize.Append(" instance)");

			yield return serialize;

			foreach (var line in ProcessGenerator.NewBracedSectionProcess().Process(GenerateSerializeMethod()))
			{
				yield return line;
			}

			var deserialize = new StringBuilder("public static bool TryDeserialize");
			deserialize.Append(_model.Identifier);
			deserialize.Append("(object[] data, out ");
			deserialize.Append(_model.Identifier);
			deserialize.Append(" instance)");

			yield return deserialize;

			foreach (var line in ProcessGenerator.NewBracedSectionProcess().Process(GenerateDeserializeMethod()))
			{
				yield return line;
			}
		}

		public IEnumerable<GeneratorItem> GenerateSerializeMethod()
		{
			foreach (var line in new SerializeCodeGenerator(_model).Generate())
			{
				yield return line;
			}
		}

		public IEnumerable<GeneratorItem> GenerateDeserializeMethod()
		{
			foreach (var line in new DeserializeCodeGenerator(_model).Generate())
			{
				yield return line;
			}
		}
	}
}