using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.Core.Tokens
{
	public class Model : IToken, IType
	{
		public bool IsValueType => false;

		public Model(string identifier, IEnumerable<Parent> parents, IEnumerable<Field> fields)
		{
			Identifier = identifier;
			Parents = parents.ToArray();
			Fields = fields.ToArray();
		}

		public string Identifier { get; set; }
		public Parent[] Parents { get; set; }
		public Field[] Fields { get; set; }
	}
}