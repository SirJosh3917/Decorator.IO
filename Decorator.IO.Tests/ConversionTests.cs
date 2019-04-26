using Decorator.IO.Core;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Decorator.IO.Tests
{
	public class ConversionTests
	{
		public static IEnumerable<object[]> Data()
		{
			var a = new Core.DecoratorClass
			{
				Name = "a"
			};

			var b = new Core.DecoratorClass
			{
				Name = "b",
				Parents = new[]
				{
					a
				}
			};

			var c = new Core.DecoratorClass
			{
				Name = "c",
				Parents = new[]
				{
					a,
					b
				}
			};

			var d = new Core.DecoratorClass
			{
				Name = "d"
			};

			var e = new Core.DecoratorClass
			{
				Name = "e"
			};

			var f = new Core.DecoratorClass
			{
				Name = "f",
				Parents = new []
				{
					d,
					c
				}
			};

			var g = new Core.DecoratorClass
			{
				Name = "g",
				Parents = new []
				{
					d,
					f
				}
			};

			var h = new Core.DecoratorClass
			{
				Name = "h",
				Parents = new[]
				{
					a,
					e
				}
			};

			yield return new object[]
			{
				@"NAMESPACE test;
a:
b [a]:
c [a, b]:
d:
e:
f [d, c]:
g [d, f]:
h [a, e]:
",
				new Core.DecoratorFile
				{
					Namespace = "test",
					Classes = new Core.DecoratorClass[]
					{
						a,
						b,
						c,
						d,
						e,
						f,
						g,
						h
					}
				}
			};
		}

		[Theory]
		[MemberData(nameof(Data))]
		public void Test(string data, DecoratorFile equivalent)
		{
			IParser parser = new Parser.DecoratorIOParser();

			var result = parser.Parse(data);

			result
				.Should()
				.BeEquivalentTo(equivalent);
		}
	}
}
