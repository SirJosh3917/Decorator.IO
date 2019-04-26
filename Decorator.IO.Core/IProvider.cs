using System.IO;

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
		void Provide(Stream outputStream, DecoratorFile file);
	}
}