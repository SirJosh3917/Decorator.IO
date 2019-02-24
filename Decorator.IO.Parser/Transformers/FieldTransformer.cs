using System;
using Decorator.IO.Core.Tokens;

using System.Collections.Generic;

namespace Decorator.IO.Parser.Transformers
{
	public class FieldTransformer : ITransformer<DIOParser.FieldContext, Field>
	{
		private readonly IEnumerable<Model> _models;
		private readonly int _counter;

		public FieldTransformer(IEnumerable<Model> models, int counter)
		{
			_models = models;
			_counter = counter;
		}

		public Field Transform(DIOParser.FieldContext input)
		{
			var txtPosition = input.position()?.NUMERIC()?.GetText();
			var txtModifier = input.modifier().MODIFIER().GetText();
			var txtType = input.type().IDENTIFIER().GetText();
			var txtLabel = input.IDENTIFIER().GetText();

			return new Field
			(
				position: Convert.ToInt32(txtPosition ?? _counter.ToString()),
				modifier: new ModifierTransformer().Transform(txtModifier),
				type: new TypeTransformer(_models).Transform(txtType),
				identifier: txtLabel
			);
		}
	}
}