using Decorator.IO.Parser;

using FluentAssertions;

using Sprache;

using Xunit;

namespace Decorator.IO.Tests
{
	public class FieldTypeParserTests
	{
		private void Test(Parser<FieldType> parser, FieldType expected, params string[] values)
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
		[InlineData(FieldType.Required, "R")]
		[InlineData(FieldType.Optional, "O")]
		public void Parse(FieldType expected, string parse)
			=> DecoratorFieldTypeParsers.FieldType
				.Parse(parse)
				.Should().Be(expected);

		[Fact]
		public void Required()
			=> Test
			(
				DecoratorFieldTypeParsers.Required,
				FieldType.Required,
				"R", "REQ", "REQUIRED"
			);

		[Fact]
		public void Optional()
			=> Test
			(
				DecoratorFieldTypeParsers.Optional,
				FieldType.Optional,
				"O", "OPT", "OPTIONAL"
			);
	}
}