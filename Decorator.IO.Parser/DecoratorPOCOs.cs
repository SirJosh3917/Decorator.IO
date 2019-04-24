using System;
using System.Collections.Generic;
using System.Text;

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
		public DecoratorField[] Fields { get; set; } = new DecoratorField[0];
	}

	public class DecoratorField
	{
		public int Index { get; set; }
		public FieldType Type { get; set; }
		public Type CSharpType { get; set; } = typeof(void);
		public string Name { get; set; } = "";
	}

	public enum FieldType
	{
		Required,
		Optional
	}
}
