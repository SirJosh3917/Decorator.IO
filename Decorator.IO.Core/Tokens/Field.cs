namespace Decorator.IO.Core.Tokens
{
	public class Field : IToken
	{
		public Field(int position, Modifier modifier, IType type, string identifier)
		{
			Position = position;
			Modifier = modifier;
			Type = type;
			Identifier = identifier;
		}

		public string Identifier { get; set; }
		public int Position { get; set; }

		public Modifier Modifier { get; set; }
		public IType Type { get; set; }
	}
}