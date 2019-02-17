using System.Text;

namespace Decorator.IO.Providers.Core
{
	public interface IApplication<T>
	{
		void Apply(T item);
	}

	public interface IStringBuilderApplication : IApplication<StringBuilder>
	{
	}
}