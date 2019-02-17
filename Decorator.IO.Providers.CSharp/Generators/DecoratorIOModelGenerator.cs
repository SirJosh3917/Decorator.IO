using Decorator.IO.Core.Tokens;
using Decorator.IO.Providers.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.Providers.CSharp.Generators
{
	public class DecoratorIOModelGenerator : IGenerator
	{
		private readonly Model _model;

		public DecoratorIOModelGenerator(Model model)
		{
			_model = model;
		}

		public IEnumerable<GeneratorItem> Generate()
		{
			var serialize = new StringBuilder("public static object[] Serialize");
			serialize.Append(_model.Identifier);
			serialize.Append("(");
			serialize.Append(_model.Identifier);
			serialize.Append(" instance)");

			yield return serialize;

			var deserialize = new StringBuilder("public static ");
			deserialize.Append(_model.Identifier);
			deserialize.Append(" Deserialize");
			deserialize.Append(_model.Identifier);
			deserialize.Append("(object[] data)");

			yield return deserialize;
		}
	}
}
