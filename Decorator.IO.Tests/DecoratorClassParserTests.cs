using Decorator.IO.Parser;

using FluentAssertions;

using Sprache;

using System.Collections.Generic;

using Xunit;

namespace Decorator.IO.Tests
{
	public class DecoratorClassParserTests
	{
		[Theory]
		[MemberData(nameof(Source))]
		public void ParsesClass(string source, DecoratorClass expected)
		{
			var result = DecoratorPocoParser.DecoratorClass
				.TryParse(source);

			result.WasSuccessful.Should().BeTrue();

			result.Value
				.Should()
				.BeEquivalentTo(expected);
		}

		public static IEnumerable<object[]> Source()
		{
			yield return new object[]
			{
				@"test:",
				new DecoratorClass
				{
					Name = "test"
				}
			};

			yield return new object[]
			{
				@"test [parent_a]:",
				new DecoratorClass
				{
					Name = "test",
					Inherits = new [] { "parent_a" }
				}
			};

			yield return new object[]
			{
				@"test [parent_a, parent_b, parent_c]:",
				new DecoratorClass
				{
					Name = "test",
					Inherits = new [] { "parent_a", "parent_b", "parent_c" }
				}
			};

			yield return new object[]
			{
				@"test:
| (0) R I a_field",
				new DecoratorClass
				{
					Name = "test",
					Fields = new DecoratorField[]
					{
						new DecoratorField
						{
							Index = 0,
							Type = FieldType.Required,
							CSharpType = typeof(int),
							Name = "a_field"
						}
					}
				}
			};

			yield return new object[]
			{
				@"test [parent_a, parent_b]:
| (0) R I a_field
| 1 O S b_field",
				new DecoratorClass
				{
					Name = "test",
					Inherits = new [] { "parent_a", "parent_b" },
					Fields = new DecoratorField[]
					{
						new DecoratorField
						{
							Index = 0,
							Type = FieldType.Required,
							CSharpType = typeof(int),
							Name = "a_field"
						},
						new DecoratorField
						{
							Index = 1,
							Type = FieldType.Optional,
							CSharpType = typeof(string),
							Name = "b_field"
						}
					}
				}
			};
		}
	}
}