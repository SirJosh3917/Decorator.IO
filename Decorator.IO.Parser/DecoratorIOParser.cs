using Decorator.IO.Core;

using Sprache;

namespace Decorator.IO.Parser
{
	public class DecoratorIOParser : IParser
	{
		public Core.DecoratorFile Parse(string file)
			=> DecoratorFileParser.FileParser
				.Parse(file)
				.Proess();
	}
}