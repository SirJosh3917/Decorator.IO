using Decorator.IO.Core;
using System;
using System.Linq;

namespace Decorator.IO.Providers.CSharp
{
	public class SerializerCode
	{
		private readonly DecoratorFile _context;

		public SerializerCode(DecoratorFile context)
		{
			_context = context;
		}

		public string Generate(DecoratorClass decoratorClass)
		{
			return $@"object[] array = new object[{GenerateSize(decoratorClass)}];
return array;";
		}

		public string GenerateSize(DecoratorClass decoratorClass)
		{
			var generator = new SizeGenerator(_context);
		}
	}
}