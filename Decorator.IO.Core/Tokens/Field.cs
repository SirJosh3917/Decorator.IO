namespace Decorator.IO.Core.Tokens
{
	public class Field
	{
		public Field(int position, Modifier modifier, IType type, string identifier)
		{
			Position = position;
			Modifier = modifier;
			Type = type;
			Identifier = identifier;
		}

		public string Identifier { get; }
		public int Position { get; }

		public Modifier Modifier { get; }
		public IType Type { get; }
	}
}