using System.IO;
using System.Threading.Tasks;

namespace Decorator.IO.Core
{
	/// <summary>
	/// Some kind of provider to make decorator files.
	/// </summary>
	public interface IProvider
	{
		/// <summary>
		/// Writes to the stream the DecoratorFile but processed.
		/// </summary>
		/// <param name="outputStream">The output stream.</param>
		/// <param name="file">The file.</param>
		Task Provide(Stream outputStream, DecoratorFile file);
	}
}