using Decorator.IO.Core;
using Sprache;

namespace Decorator.IO.Parser
{
	public static class DecoratorModifierParsers
	{
		public static readonly Parser<Modifier> Required =
			from @required in Parse.String("REQUIRED")
				.Or(Parse.String("REQ"))
				.Or(Parse.String("R"))
			select Modifier.Required;

		public static readonly Parser<Modifier> Optional =
			from @required in Parse.String("OPTIONAL")
				.Or(Parse.String("OPT"))
				.Or(Parse.String("O"))
			select Modifier.Optional;

		public static readonly Parser<Modifier> FieldType =
			from Modifier in Required
				.Or(Optional)
			select Modifier;
	}
}