using Decorator.IO.Core.Tokens;
using Decorator.IO.Providers.Core;
using Decorator.IO.Providers.CSharp.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decorator.IO.Providers.CSharp.Generators
{
	public class DecoratorIOClassGenerator : IGenerator
	{
		private readonly Model[] _models;

		public DecoratorIOClassGenerator(Model[] models)
		{
			_models = models;
		}

		public IEnumerable<GeneratorItem> Generate()
		{
			return new ClassProcess
			(
				name: "DecoratorIO",
				modifiers: "public static",
				inherit: Array.Empty<string>(),
				isInterface: false
			)
			.Process
			(
				IModelInterface()
				.Concat(new DecoratorIOModelsGenerator(_models).Generate())
			)
			.Select(x => (GeneratorItem)x);
		}

		public static IEnumerable<GeneratorItem> IModelInterface()
		{
			yield return "public interface IModel<T> where T : IModel<T>";
			yield return "{";
			yield return "\tobject[] Serialize();";
			yield return "}";
		}
	}
}
