using FluentAssertions;

using Xunit;

namespace Decorator.IO.Tests.LangParsingTests
{
	public class ModelsTests : Test
	{
		[Fact]
		public void TwoModelsWithParentsNextToEachother()
		{
			var parser = this.GetDIOParser(@"
a [a, b]:
b [a, b]:
");

			var models = parser.models().model();

			models.Length.Should().Be(2);
		}

		[Fact]
		public void GetModel()
		{
			var models = this.GetDIOParser(@"

my_model:
| $R #INT some_field
| (2) $R #INT another_field
| (3) $R #INT another_field
| (18) $R #INT yet_another_field
| $R #INT yEEt

another_model:
| $R #INT yote
| $R #INT yate
| $R #INT yeet

some_model:
blank_models [some_parent]:
blank_model_ftw [another_parent, wait_even_another]:
with_field:
| (123) $FA #STR cheap

no_fields:

")
			.models();

			models.model(0) // see ModelTests line 69 for adding + 1 to the length, (it's a /shrug thing)
				.fields().field().Length.Should().Be(5);

			models.model(1)
				.fields().field().Length.Should().Be(3);

			var fs = models.model(2)
				.fields().Should().BeNull();

			var m3 = models.model(3);

			m3.fields().Should().BeNull();
			m3.model_identifier().model_inherit().inheriters().inherit().Length.Should().Be(1);

			var m4 = models.model(4);

			m4.fields().Should().BeNull();
			m4.model_identifier().model_inherit().inheriters().inherit().Length.Should().Be(2);

			var m5 = models.model(5).fields().field();
			m5.Length.Should().Be(1);
			m5[0].modifier().MODIFIER().Symbol.Text.Should().Be("FA");
			m5[0].type().IDENTIFIER().Symbol.Text.Should().Be("STR");

			models.model(6)
				.fields().Should().BeNull();
		}
	}
}