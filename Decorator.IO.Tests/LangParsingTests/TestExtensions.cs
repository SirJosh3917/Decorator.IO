using Decorator.IO.Parser;

namespace Decorator.IO.Tests.LangParsingTests
{
	public abstract class Test
	{
	}

	public static class TestExtensions
	{
		public static DIOParser GetDIOParser(this Test test, string input)
			=> GenParser.GetParser(new Antlr4.Runtime.AntlrInputStream(input));
	}
}