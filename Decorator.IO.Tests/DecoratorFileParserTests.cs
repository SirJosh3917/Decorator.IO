using Decorator.IO.Parser;
using FluentAssertions;
using Sprache;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Decorator.IO.Tests
{
	public class DecoratorFileParserTests
	{
		[Theory]
		[MemberData(nameof(Source))]
		public void ParsesClass(string source, DecoratorFile expected)
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
				new DecoratorFile
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
									Type = FieldType.Required,
									CSharpType = typeof(int),
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
									Type = FieldType.Optional,
									CSharpType = typeof(string),
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
