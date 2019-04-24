using System;

namespace Decorator.IO.Core
{
	/// <summary>
	/// A field for the item.
	/// </summary>
	public class DecoratorField
	{
		/// <summary>
		/// The name of the field.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The type of the field.
		/// </summary>
		public Type Type { get; set; }

		/// <summary>
		/// The modifier.
		/// </summary>
		public Modifier Modifier { get; set; }

		/// <summary>
		/// The index of a field.
		/// </summary>
		public int Index { get; set; }

		public override string ToString() => $@"@{Index}: {Modifier} {Type} ""{Name}""";
	}
}