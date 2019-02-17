namespace Decorator.IO.Core.Tokens
{
	public class Parent
	{
		public Parent(string identifier) => Identifier = identifier;

		public string Identifier { get; }
	}
}