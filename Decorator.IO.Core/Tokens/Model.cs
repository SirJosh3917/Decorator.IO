using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.Core.Tokens
{
	public class Model : IToken, IType
	{
		public Model(string identifier, IEnumerable<Parent> parents, IEnumerable<Field> fields)
		{
			Identifier = identifier;
			Parents = parents.ToArray();
			Fields = fields.ToArray();
		}

		public string Identifier { get; }
		public Parent[] Parents { get; }
		public Field[] Fields { get; }
	}
}