namespace Decorator.IO.Core.Tokens
{
	public class Parent : IToken
	{
		public Parent(Model model) => Model = model;

		public Model Model { get; set; }
	}
}