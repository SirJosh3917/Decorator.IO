using Decorator.IO.Core.Tokens;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.Parser.Transformers
{
	public class ModelTransformer : ITransformer<DIOParser.ModelContext, Model>
	{
		private readonly IEnumerable<Model> _models;

		public ModelTransformer(IEnumerable<Model> models) => _models = models;

		public Model Transform(DIOParser.ModelContext input)
		{
			var identifier = input.model_identifier();

			var name = identifier.IDENTIFIER().GetText();
			var parents = GetParents(identifier);
			var fields = TransformFields(input.fields()).ToArray();

			return new Model
			(
				identifier: name,
				parents: parents,
				fields: fields
			);
		}

		public Parent[] GetParents(DIOParser.Model_identifierContext modelIdentifierCtx)
		{
			var parents = modelIdentifierCtx.model_inherit();

			if (parents == null) return Array.Empty<Parent>();

			return TransformParents(parents.inheriters()).ToArray();
		}

		public IEnumerable<Parent> TransformParents(DIOParser.InheritersContext inheritersContext)
		{
			return inheritersContext.inherit()
				.Select
				(
					parent => new ParentTransformer(_models).Transform(parent)
				);
		}

		public IEnumerable<Field> TransformFields(DIOParser.FieldsContext fieldsContext)
		{
			var counter = 0;

			foreach (var fieldCtx in fieldsContext.field())
			{
				yield return new FieldTransformer(_models, counter).Transform(fieldCtx);

				counter++;
			}
		}
	}
}