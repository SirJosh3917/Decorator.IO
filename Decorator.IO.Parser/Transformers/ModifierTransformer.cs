using Decorator.IO.Core.Tokens;

using System;

namespace Decorator.IO.Parser
{
	public class ModifierTransformer : ITransformer<string, Modifier>
	{
		public Modifier Transform(string input)
		{
			switch (input)
			{
				case "REQUIRED":
				case "REQ":
				case "R":
					return Modifier.Required;

				case "OPTIONAL":
				case "OPT":
				case "O":
					return Modifier.Optional;

				case "FLATTEN":
				case "FLT":
				case "F":
					return Modifier.Flatten;

				case "ARRAY":
				case "ARR":
				case "A":
					return Modifier.Array;

				case "FLATTEN_ARRAY":
				case "FLT_ARR":
				case "FA":
					return Modifier.FlattenArray;

				default: throw new Exception($"Unknown modifier {input}");
			}
		}
	}
}