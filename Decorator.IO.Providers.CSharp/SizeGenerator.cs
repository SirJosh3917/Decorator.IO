using Decorator.IO.Core;

namespace Decorator.IO.Providers.CSharp
{
	public class SizeGenerator
	{
		private readonly DecoratorFile _context;

		public SizeGenerator(DecoratorFile context)
		{
			_context = context;
		}

		public string GenerateSize(DecoratorField field, string objectContext)
		{
			return "+ 1";
		}
	}
}