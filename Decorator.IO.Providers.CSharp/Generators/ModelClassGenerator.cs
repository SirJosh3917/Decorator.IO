using Decorator.IO.Core.Tokens;
using Decorator.IO.Providers.Core;
using Decorator.IO.Providers.CSharp.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decorator.IO.Providers.CSharp.Generators
{
	public class ModelClassGenerator : IGenerator
	{
		private readonly Model _model;

		public ModelClassGenerator(Model model)
		{
			_model = model;
		}

		public IEnumerable<GeneratorItem> Generate()
			=> new ClassProcess
			(
				name: _model.Identifier,
				modifiers: "public",
				inherit: _model.Parents.Select(parent => $"I{parent.Model.Identifier}").Prepend($"DecoratorIO.IModel<I{_model.Identifier}>"),
				isInterface: true
			)
			.Process
			(
				GenerateInterfaceProperties(_model)
			)
			.Concat
			(
				new ClassProcess
				(
					name: _model.Identifier,
					modifiers: "public",
					inherit: new[] { $"I{_model.Identifier}" },
					isInterface: false
				)
				.Process
				(
					GenerateClassProperties(_model)
				)
			)
			.Select(x => (GeneratorItem)x);

		public static IEnumerable<GeneratorItem> GenerateInterfaceProperties(Model model)
			=> new PropertiesGenerator
			(
				fields: model.Fields,
				addPublic: false
			)
			.Generate();

		public static IEnumerable<GeneratorItem> GenerateClassProperties(Model model)
		{
			var properties = new PropertiesGenerator
			(
				fields: model.Fields,
				addPublic: true
			)
			.Generate();

			foreach(var parent in model.Parents)
			{
				properties.Concat(GenerateClassProperties(parent.Model));
			}

			return properties;
		}
	}
}
