using Decorator.IO.Core.Tokens;

using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.Parser.Transformers
{
	public class ModelsTransformer : ITransformer<DIOParser.ModelsContext, Namespace>
	{
		private readonly List<Model> _models;

		public ModelsTransformer(List<Model> models = default) => _models = models ?? new List<Model>();

		public Namespace Transform(DIOParser.ModelsContext input)
		{
			var nsName = new NamespaceTransformer().Transform(input.name_space());
			var models = input.model().Select(model => ReadModel(model));

			return new Namespace
			(
				name: nsName,
				models: models.ToArray()
			);
		}

		public Model ReadModel(DIOParser.ModelContext modelContext)
		{
			var result = new ModelTransformer(_models).Transform(modelContext);

			_models.Add(result);

			return result;
		}
	}
}