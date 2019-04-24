namespace Decorator.IO.Core
{
	/// <summary>
	/// The modifier for a decorator item.
	/// </summary>
	public enum Modifier
	{
		/// <summary>
		/// This item is required, it has to exist.
		/// </summary>
		Required,

		/// <summary>
		/// This can optionally have a value.
		/// </summary>
		Optional
	}
}