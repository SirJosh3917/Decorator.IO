using System;

namespace Decorator.IO.Parser
{
	public class DecoratorFile
	{
		public string Namespace { get; set; }
		public DecoratorClass[] Classes { get; set; }
	}

	public class DecoratorClass
	{
		public string[] Inherits { get; set; } = new string[0];
		public string Name { get; set; } = "";
		public Core.DecoratorField[] Fields { get; set; } = new Core.DecoratorField[0];
	}
}