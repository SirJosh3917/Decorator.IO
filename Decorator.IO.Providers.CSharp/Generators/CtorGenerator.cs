using Decorator.IO.Core.Tokens;
using Decorator.IO.Providers.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.Providers.CSharp.Generators
{
	public class CtorGenerator : IGenerator
	{
		private readonly Model _model;

		public CtorGenerator(Model model)
		{
			_model = model;
		}

		public IEnumerable<GeneratorItem> Generate()
		{
			var strb = new StringBuilder();

			strb.Append("public ");
			strb.Append(_model.Identifier);
			strb.Append("()");

			yield return strb;
			yield return "{";
			yield return "}";
		}
	}
}
