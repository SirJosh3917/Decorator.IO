using FluentAssertions;

using Xunit;

namespace Decorator.IO.Tests.LangParsingTests
{
	public class MultipleFields : Test
	{
		[Fact]
		public void ParsesSingleField()
		{
			var parser = this.GetDIOParser(@"| REQUIRED STR single_field");

			var field = parser.fields()
				.field(0);

			field.IDENTIFIER().Symbol.Text
				.Should().Be("single_field");

			field.modifier().MODIFIER().Symbol.Text
				.Should().Be("REQUIRED");

			field.type().IDENTIFIER().Symbol.Text
				.Should().Be("STR");
		}

		[Fact]
		public void ParsesFields()
		{
			var parser = this.GetDIOParser(@"| R STR single_field
| (2) R STR another_field
| (8) O INT yet_another_field");

			var fields = parser.fields();

			fields
				.field(0)
				.IDENTIFIER().Symbol.Text
				.Should().Be("single_field");

			var f1 = fields.field(1);

			f1.IDENTIFIER().Symbol.Text
				.Should().Be("another_field");

			f1.position().NUMERIC().Symbol.Text
				.Should().Be("2");

			f1.modifier().MODIFIER().Symbol.Text
				.Should().Be("R");

			f1.type().IDENTIFIER().Symbol.Text
				.Should().Be("STR");

			var f2 = fields.field(2);

			f2.IDENTIFIER().Symbol.Text
				.Should().Be("yet_another_field");

			f2.position().NUMERIC().Symbol.Text
				.Should().Be("8");

			f2.modifier().MODIFIER().Symbol.Text
				.Should().Be("O");

			f2.type().IDENTIFIER().Symbol.Text
				.Should().Be("INT");
		}
	}

	public class FieldTests : Test
	{
		[Fact]
		public void ParsesFieldWithPosition()
		{
			var parser = this.GetDIOParser(@"| (1) R STR my_field");

			var field = parser.field();

			field.IDENTIFIER()
				.Symbol.Text
				.Should()
				.Be("my_field");

			field.position().NUMERIC()
				.Symbol.Text
				.Should()
				.Be("1");

			field.modifier().MODIFIER()
				.Symbol.Text
				.Should()
				.Be("R");

			field.type().IDENTIFIER()
				.Symbol.Text
				.Should()
				.Be("STR");
		}

		[Fact]
		public void ParsesFieldWithoutPosition()
		{
			var parser = this.GetDIOParser(@"| my_field");

			var field = parser.field();

			field.IDENTIFIER()
				.Symbol.Text
				.Should()
				.Be("my_field");

			field.position()
				.Should()
				.BeNull();
		}
	}
}