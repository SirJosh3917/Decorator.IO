using Decorator.IO.Parser;
using FluentAssertions;
using Sprache;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Decorator.IO.Tests
{
	public class DecoratorFieldParser
	{
		[Theory]
		[MemberData(nameof(A))]
		public void ParseField(string data, DecoratorField expected)
		{
			var result = LanguageParsers.DecoratorField
				.TryParse(data);

			result.WasSuccessful
				.Should().Be(true);

			result.Value
				.Should()
				.BeEquivalentTo(expected);
		}

		public static IEnumerable<object[]> A()
		{
			yield return new object[]
			{
				"| (0) R I some_thing",
				new DecoratorField
				{
					Index = 0,
					Type = FieldType.Required,
					CSharpType = typeof(int),
					Name = "some_thing"
				}
			};
		}
	}
}
