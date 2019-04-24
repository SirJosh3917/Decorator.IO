using System.Linq;

namespace Decorator.IO.Core
{
	/// <summary>
	/// A decorator file.
	/// </summary>
	public class DecoratorFile
	{
		/// <summary>
		/// The namespace defined.
		/// </summary>
		public string Namespace { get; set; }

		/// <summary>
		/// The classes that the file contains.
		/// </summary>
		public DecoratorClass[] Classes { get; set; }

		public override string ToString() => $"NAMESPACE {Namespace}:\n"
			+ Classes.Select(x => x.ToString()).Aggregate((a, b) => $"\n{a}\n{b}");
	}
}