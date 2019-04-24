using Decorator.IO.Parser;
using FluentAssertions;
using Sprache;
using Xunit;

namespace Decorator.IO.Tests
{
	public class NameParserTests
	{
		[Theory]
		[InlineData(true, "an_example_name")]
		[InlineData(true, " whitespace in the middle ", "whitespace")]
		[InlineData(false, "!")]
		[InlineData(true, "  with_whiteSpace  ", "with_whiteSpace")]
		[InlineData(true, "\r\n\t\n\r a \r\t\n\t\r ", "a")]
		public void ParsesName(bool should, string name, string expected = null)
		{
			expected ??= name;

			var result = LanguageParsers.Identifier
				  .TryParse(name);

			result.WasSuccessful
				.Should()
				.Be(should);

			if (should)
			{
				result.Value.Should().Be(expected);
			}
		}
	}
}
