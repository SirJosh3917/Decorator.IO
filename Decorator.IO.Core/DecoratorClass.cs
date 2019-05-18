using System.Linq;

namespace Decorator.IO.Core
{
	/// <summary>
	/// A class for a decorator item.
	/// </summary>
	public class DecoratorClass
	{
		/// <summary>
		/// The name of it.
		/// </summary>
		public string Name { get; set; }

		public string RawName { get; set; }

		/// <summary>
		/// The parents.
		/// </summary>
		public DecoratorClass[] Parents { get; set; } = new DecoratorClass[0];

		/// <summary>
		/// The fields it has.
		/// </summary>
		public DecoratorField[] Fields { get; set; } = new DecoratorField[0];

		public override string ToString() => $"Class: {Name}\n"
			+ Fields.Select(x => x.ToString()).Aggregate((a, b) => $"\t{a}\n\t{b}")
			+ Parents.Select(x => x.ToString()).Aggregate((a, b) => $"\t\t{a}\n\t\t{b}");
	}
}