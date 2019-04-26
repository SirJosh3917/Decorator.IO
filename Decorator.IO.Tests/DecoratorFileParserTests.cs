using Decorator.IO.Core;
using Decorator.IO.Parser;

using FluentAssertions;

using Sprache;

using System.Collections.Generic;

using Xunit;
using DecoratorClass = Decorator.IO.Parser.DecoratorClass;

namespace Decorator.IO.Tests
{
	public class DecoratorFileParserTests
	{
		[Theory]
		[MemberData(nameof(Source))]
		public void ParsesClass(string source, IO.Parser.DecoratorFile expected)
		{
			var result = DecoratorFileParser.FileParser
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
				@"NAMESPACE my_app;
test1 [parent]:
| 0 R I a

test2:
| 1 O S b",
				new IO.Parser.DecoratorFile
				{
					Namespace = "my_app",
					Classes = new DecoratorClass[]
					{
						new DecoratorClass
						{
							Name = "test1",
							Inherits = new [] { "parent" },
							Fields = new DecoratorField[]
							{
								new DecoratorField
								{
									Index = 0,
									Modifier = Modifier.Required,
									Type = typeof(int),
									Name = "a"
								}
							}
						},
						new DecoratorClass
						{
							Name = "test2",
							Fields = new DecoratorField[]
							{
								new DecoratorField
								{
									Index = 1,
									Modifier = Modifier.Optional,
									Type = typeof(string),
									Name = "b"
								}
							}
						}
					}
				}
			};
		}
	}
}