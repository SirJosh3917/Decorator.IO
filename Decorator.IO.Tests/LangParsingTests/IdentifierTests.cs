using FluentAssertions;

using Xunit;

namespace Decorator.IO.Tests.LangParsingTests
{
	public class IdentifierTests : Test
	{
		[Fact]
		public void ValidCharsPass()
		{
			const string id = "_qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";

			this.GetDIOParser(id)
				.tokens_IDENTIFIER()
				.IDENTIFIER()
				.Symbol.Text
				.Should()
				.Be(id);
		}

		[Fact]
		public void NothingElse()
		{
			foreach (var chr in "0123456789")
			{
				this.GetDIOParser(chr.ToString())
					.tokens_IDENTIFIER()
					.IDENTIFIER()
					.Should()
					.Be(null);
			}
		}
	}
}