using Decorator.IO.Core.Tokens;
using Decorator.IO.Providers.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decorator.IO.Providers.CSharp.Generators
{
	public class ModelsGenerator : IGenerator
	{
		private readonly Model[] _models;

		public ModelsGenerator(Model[] models)
		{
			_models = models;
		}

		public IEnumerable<GeneratorItem> Generate()
		{
			return _models
				.SelectMany
				(
					model => new ModelClassGenerator(model)
						.Generate()
				);
		}
	}
}
