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
		[Fact]
		public void Required()
		{
			DecoratorFieldTypeParsers.Required
				.Parse("R")
				.Should().Be(DecoratorType.Required);

			DecoratorFieldTypeParsers.Required
				.Parse("REQ")
				.Should().Be(DecoratorType.Required);

			DecoratorFieldTypeParsers.Required
				.Parse("REQUIRED")
				.Should().Be(DecoratorType.Required);

			DecoratorFieldTypeParsers.Required
				.TryParse("anythign else")
				.WasSuccessful
				.Should().Be(false);
		}
	}
}
