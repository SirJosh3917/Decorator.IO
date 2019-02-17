using Antlr4.Runtime;

namespace Decorator.IO.Parser
{
	public static class GenParser
	{
		public static DIOParser GetParser(AntlrInputStream input)
			=> new DIOParser(new CommonTokenStream(new DIOLexer(input)));
	}
}