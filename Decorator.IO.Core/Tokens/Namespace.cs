namespace Decorator.IO.Core.Tokens
{
	public class Namespace : IToken
	{
		public Namespace(string name, Model[] models)
		{
			Name = name;
			Models = models;
		}

		public string Name { get; set; }
		public Model[] Models { get; set; }
	}
}