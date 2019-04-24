using Decorator.IO.Parser;
using FluentAssertions;
using Sprache;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Decorator.IO.Tests
{
	public class DecoratorFieldTypeParserTests
	{
		private void Test(Parser<DecoratorType> parser, DecoratorType expected, params string[] values)
		{
			foreach(var i in values)
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
		[InlineData(DecoratorType.Required, "R")]
		[InlineData(DecoratorType.Optional, "O")]
		public void Parse(DecoratorType expected, string parse)
			=> DecoratorFieldTypeParsers.FieldType
				.Parse(parse)
				.Should().Be(expected);

		[Fact]
		public void Required()
			=> Test
			(
				DecoratorFieldTypeParsers.Required,
				DecoratorType.Required,
				"R", "REQ", "REQUIRED"
			);

		[Fact]
		public void Optional()
			=> Test
			(
				DecoratorFieldTypeParsers.Optional,
				DecoratorType.Optional,
				"O", "OPT", "OPTIONAL"
			);
	}
}
