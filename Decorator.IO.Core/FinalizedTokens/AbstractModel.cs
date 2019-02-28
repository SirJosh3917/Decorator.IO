namespace Decorator.IO.Core.FinalizedTokens
{
	public class AbstractModel
	{
		public string Identifier { get; }

		public AbstractModel[] Parents { get; }

		public Field[] Fields { get; }

		public AbstractModel(string identifier, AbstractModel[] parents, Field[] fields)
		{
			Identifier = identifier;
			Parents = parents;
			Fields = fields;
		}
	}
}