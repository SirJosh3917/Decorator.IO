using Decorator.IO.Core;

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
			return $@"{Config.ArrayName}[{counterName}] = {objectContext}{decoratorField.Name};
{counterName}++;";
		}
	}
}