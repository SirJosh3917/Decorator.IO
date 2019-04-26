namespace Decorator.IO.Core
{
	/// <summary>
	/// Parses some text into a file.
	/// </summary>
	public interface IParser
	{
		/// <summary>
		/// Parses out a file.
		/// </summary>
		/// <param name="file">The source of the file</param>
		DecoratorFile Parse(string file);
	}
}
