using FluentAssertions;

using Xunit;

namespace Decorator.IO.Tests.LangParsingTests
{
	public class PositionTests : Test
	{
		[Fact]
		public void ParsesPosition()
		{
			var parser = this.GetDIOParser(@"(1234)");

			parser.position().NUMERIC().Symbol.Text
				.Should().Be("1234");
		}
	}
}