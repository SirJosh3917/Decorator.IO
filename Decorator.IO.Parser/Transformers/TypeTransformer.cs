using Decorator.IO.Core;
using Decorator.IO.Core.Tokens;

using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.Parser
{
	public class TypeTransformer : ITransformer<string, IType>
	{
		private readonly IEnumerable<Model> _models;

		public TypeTransformer(IEnumerable<Model> models) => _models = models;

		public IType Transform(string type)
		{
			switch (type)
			{
				case "STRING":
				case "STR":
				case "S":
					return new StringType();

				case "INTEGER":
				case "INT":
				case "I":
					return new IntegerType();

				case "UNSIGNED_INTEGER":
				case "U_INTEGER":
				case "UINT":
				case "UI":
					return new UnsignedIntegerType();

				case "BYTE":
				case "BYT":
				case "B":
					return new ByteType();

				default: return _models.First(x => x.Identifier == type);
			}
		}
	}
}