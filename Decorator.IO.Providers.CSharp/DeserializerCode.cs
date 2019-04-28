using System;
using Decorator.IO.Core;

namespace Decorator.IO.Providers.CSharp
{
	internal class DeserializerCode
	{
		private DecoratorFile _context;

		public DeserializerCode(DecoratorFile context) => _context = context;

		public string Generate(DecoratorClass decoratorClass)
		{
			var fields = decoratorClass.AllFieldsOf();

			if (fields.Length == 0)
			{
				return $"return new {decoratorClass.Name}();";
			}

			return $@"var current = new {decoratorClass.Name}();

return current;";
		}
	}
}