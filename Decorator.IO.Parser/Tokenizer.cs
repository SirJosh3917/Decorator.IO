using Antlr4.Runtime.Misc;

using Decorator.IO.Core.Tokens;
using Decorator.IO.Parser.Transformers;

namespace Decorator.IO.Parser
{
	public class Tokenizer : DIOBaseVisitor<IToken>
	{
		public override IToken VisitModels([NotNull] DIOParser.ModelsContext context)
		{
			return new ModelsTransformer().Transform
			(
				input: context
			);
		}
	}
}