using Decorator.IO.Core;
using Decorator.IO.Parser;

using FluentAssertions;

using Sprache;

using Xunit;

namespace Decorator.IO.Tests
{
	public class ModifierParserTests
	{
		private void Test(Parser<Modifier> parser, Modifier expected, params string[] values)
		{
			foreach (var i in values)
			{
				parser.Parse(i)
					.Should().Be(expected);
			}

			parser
				.TryParse("anythign else")
				.WasSuccessful
				.Should().Be(false);
		}

		[Theory]
		[InlineData(Modifier.Required, "R")]
		[InlineData(Modifier.Optional, "O")]
		public void Parse(Modifier expected, string parse)
			=> DecoratorModifierParsers.FieldType
				.Parse(parse)
				.Should().Be(expected);

		[Fact]
		public void Required()
			=> Test
			(
				DecoratorModifierParsers.Required,
				Modifier.Required,
				"R", "REQ", "REQUIRED"
			);

		[Fact]
		public void Optional()
			=> Test
			(
				DecoratorModifierParsers.Optional,
				Modifier.Optional,
				"O", "OPT", "OPTIONAL"
			);
	}
}