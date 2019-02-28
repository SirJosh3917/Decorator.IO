namespace Decorator.IO.Core.FinalizedTokens
{
	public class Field
	{
		public IType Type { get; }

		public string Identifier { get; }

		public int Position { get; }

		public Field(string identifier, IType type, int position)
		{
			Identifier = identifier;
			Type = type;
			Position = position;
		}
	}
}