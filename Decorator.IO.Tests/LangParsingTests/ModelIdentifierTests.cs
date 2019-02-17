using FluentAssertions;

using Xunit;

namespace Decorator.IO.Tests.LangParsingTests
{
	public class ModelIdentifierTests : Test
	{
		[Fact]
		public void CanParse()
			=> this.GetDIOParser(@"some_identifier:")
			.model_identifier()
			.IDENTIFIER()
			.Symbol.Text
			.Should()
			.Be("some_identifier");
	}
}