using System;
using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.Core.Tokens
{
	public class Namespace
	{
		public Namespace(string name, Model[] models)
		{
			Name = name;
			Models = models;
		}

		public string Name { get; }
		public Model[] Models { get; }
	}
}
