namespace Decorator.IO.Core.FinalizedTokens
{
	public class Namespace
	{
		public string Identifier { get; }

		public AbstractModel[] AbstractModels { get; }

		public Model[] Models { get; }

		public Namespace(string identifier, AbstractModel[] abstractModels, Model[] models)
		{
			Identifier = identifier;
			AbstractModels = abstractModels;
			Models = models;
		}
	}
}