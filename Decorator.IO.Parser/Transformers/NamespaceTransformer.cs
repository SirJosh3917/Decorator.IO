namespace Decorator.IO.Parser.Transformers
{
	public class NamespaceTransformer : ITransformer<DIOParser.Name_spaceContext, string>
	{
		public string Transform(DIOParser.Name_spaceContext input)

			// children[0] - "NAMESPACE"
			// children[1] - *.? everything up until the ;
			// children[2] - ;
			=> input.children[1].GetText();
	}
}