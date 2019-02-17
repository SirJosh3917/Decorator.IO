using FluentAssertions;

using Xunit;

namespace Decorator.IO.Tests.LangParsingTests
{
	public class NumberTests : Test
	{
		[Fact]
		public void ValidCharsPass()
		{
			const string id = "0123456789";

			this.GetDIOParser(id)
				.tokens_NUMERIC()
				.NUMERIC()
				.Symbol.Text
				.Should()
				.Be(id);
		}

		[Fact]
		public void NothingElse()
		{
			foreach (var chr in "_qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM")
			{
				this.GetDIOParser(chr.ToString())
					.tokens_NUMERIC()
					.NUMERIC()
					.Should()
					.Be(null);
			}
		}
	}
}