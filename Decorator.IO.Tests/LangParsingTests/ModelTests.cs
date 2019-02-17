using FluentAssertions;

using Xunit;

namespace Decorator.IO.Tests.LangParsingTests
{
	public class ModelTests : Test
	{
		[Fact]
		public void ParsesEmptyModel()
		{
			this.GetDIOParser(@"example_model:")
				.model()
				.model_identifier()
				.IDENTIFIER()
				.Symbol.Text
				.Should()
				.Be("example_model");
		}

		[Fact]
		public void ParsesModelWithOneField()
		{
			var parser = this.GetDIOParser(@"
sample_model:
| (0) $R #STR my_field
");

			var model = parser.model();

			model.model_identifier().IDENTIFIER().Symbol.Text
				.Should().Be("sample_model");

			var fields = model.fields();

			var f0 = fields.field(0);

			f0.IDENTIFIER().Symbol.Text.Should().Be("my_field");
			f0.position().NUMERIC().Symbol.Text.Should().Be("0");
			f0.modifier().MODIFIER().Symbol.Text.Should().Be("R");
			f0.type().IDENTIFIER().Symbol.Text.Should().Be("STR");
		}

		[Fact]
		public void ParsesModelWithSomeFields()
		{
			var parser = this.GetDIOParser(@"
sample_model:
| (0) $R #STR my_field
| (1) $O #INT another_field
| $F #STR im_not_testing_this
");

			var model = parser.model();

			model.model_identifier().IDENTIFIER().Symbol.Text
				.Should().Be("sample_model");

			var fields = model.fields();
			var allFields = fields.field();

			// /shrug
			allFields.Length.Should().Be(3);
		}

		[Fact]
		public void ParsesWithSingleInheritParent()
		{
			var parser = this.GetDIOParser(@"
some_model [parent]:
");

			var model = parser.model();

			model.model_identifier().IDENTIFIER().Symbol.Text
				.Should().Be("some_model");

			model.model_identifier().model_inherit().inheriters().inherit(0).IDENTIFIER().Symbol.Text
				.Should().Be("parent");
		}

		[Fact]
		public void ParsesWithSomeParents()
		{
			var parser = this.GetDIOParser(@"
some_model [parent, parent_two, parent_three]:
");

			var model = parser.model();

			model.model_identifier().IDENTIFIER().Symbol.Text
				.Should().Be("some_model");

			model.model_identifier().model_inherit().inheriters().inherit(0).IDENTIFIER().Symbol.Text
				.Should().Be("parent");

			model.model_identifier().model_inherit().inheriters().inherit(1).IDENTIFIER().Symbol.Text
				.Should().Be("parent_two");

			model.model_identifier().model_inherit().inheriters().inherit(2).IDENTIFIER().Symbol.Text
				.Should().Be("parent_three");
		}
	}
}