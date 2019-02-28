namespace Decorator.IO.Core.FinalizedTokens
{
	public class Model : IBuiltinType
	{
		public bool IsValueType => false;

		public string Identifier { get; }

		public AbstractModel Parent { get; }

		public Field[] Fields { get; }

		public Model(string identifier, AbstractModel parent, Field[] fields)
		{
			Identifier = identifier;
			Parent = parent;
			Fields = fields;
		}
	}
}