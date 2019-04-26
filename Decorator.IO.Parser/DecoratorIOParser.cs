using Decorator.IO.Core;
using Sprache;
using System;
using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.Parser
{
	public class DecoratorIOParser : IParser
	{
		public Core.DecoratorFile Parse(string file)
			=> DecoratorFileParser.FileParser
				.Parse(file)
				.Proess();
	}
}
