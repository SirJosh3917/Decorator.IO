using Sprache;

namespace Decorator.IO.Parser
{
	public static partial class LanguageParsers
	{
		public static readonly Parser<FieldType> Required =
			from @required in Parse.String("REQUIRED")
				.Or(Parse.String("REQ"))
				.Or(Parse.String("R"))
			select Parser.FieldType.Required;

		public static readonly Parser<FieldType> Optional =
			from @required in Parse.String("OPTIONAL")
				.Or(Parse.String("OPT"))
				.Or(Parse.String("O"))
			select Parser.FieldType.Optional;

		public static readonly Parser<FieldType> FieldType =
			from fieldType in Required
				.Or(Optional)
			select fieldType;
	}
}