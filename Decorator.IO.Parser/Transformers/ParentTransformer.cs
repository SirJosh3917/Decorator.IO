using Decorator.IO.Core.Tokens;

using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.Parser.Transformers
{
	public class ParentTransformer : ITransformer<DIOParser.InheritContext, Parent>
	{
		private readonly IEnumerable<Model> _models;

		public ParentTransformer(IEnumerable<Model> models) => _models = models;

		public Parent Transform(DIOParser.InheritContext input)
		{
			var txtLabel = input.IDENTIFIER().GetText();

			return new Parent
			(
				model: _models.First(x => x.Identifier == txtLabel)
			);
		}
	}
}