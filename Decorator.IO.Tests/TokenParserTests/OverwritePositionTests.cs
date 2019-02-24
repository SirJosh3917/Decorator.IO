using System;
using System.Collections.Generic;
using System.Text;
using Decorator.IO.Core;
using Decorator.IO.Core.Tokens;
using Decorator.IO.Parser;
using FluentAssertions;
using Xunit;

namespace Decorator.IO.Tests.TokenParserTests
{
	public class OverwritePositionTests
	{
		[Fact]
		public void Overwrites()
		{
			var parser = GenParser.GetParser(new Antlr4.Runtime.AntlrInputStream(@"

namespace TEST;

type_a:
| (0) R I integer_field

type_b [type_a]:
| (1) R I integer_field

"));
			var visitor = new Tokenizer();

			var ns = visitor.VisitModels(parser.models()) as Namespace;

			var modela = new Model("type_a", new Parent[]{}, new []{ new Field(0, Modifier.Required, new IntegerType(), "integer_field"),  });
			var modelb = new Model("type_b", new []{new Parent(modela), }, new[] {new Field(1, Modifier.Required, new IntegerType(), "integer_field"), });

			ns.Models
				.Should()
				.BeEquivalentTo(new Model[]
				{
					modela,
					modelb
				});
		}
	}
}
