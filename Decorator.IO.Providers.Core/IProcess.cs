using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.Providers.Core
{
	public interface IProcess<T>
	{
		IEnumerable<T> Process(IEnumerable<T> enumerable);
	}

	public interface IStringProcess : IProcess<StringBuilder>
	{
	}
}