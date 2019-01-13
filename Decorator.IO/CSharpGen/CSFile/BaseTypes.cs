namespace Decorator.IO.CSharpGen.Writer
{
	public abstract class Nameable
	{
		protected Nameable()
		{
		}

		public string Name { get; set; }
	}

	public abstract class Child<T> : Nameable
	{
		public T Parent { get; set; }
	}
}