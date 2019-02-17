using System.Collections.Generic;

namespace Decorator.IO.Providers.Core
{
	public interface IGenerator
	{
		IEnumerable<GeneratorItem> Generate();
	}
}