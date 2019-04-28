using Decorator.IO.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.Providers.CSharp
{
	public class AssignGenerator
	{
		private readonly DecoratorFile _context;

		public AssignGenerator(DecoratorFile context)
		{
			_context = context;
		}

		public string Assign(DecoratorField decoratorField, string objectContext, string counterName)
		{
			return $@"{objectContext}{decoratorField.Name};
{counterName}++;";
		}
	}
}
