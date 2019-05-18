using Decorator.IO.Core;

using Sprache;

using System.Linq;

namespace Decorator.IO.Parser
{
	public static class DecoratorPocoParser
	{
		public static readonly Parser<DecoratorField> DecoratorField =
			from _ in Parse.Char('|').Once().Token()
			from number in CoreParser.DecoratorNumber.Token()
			from Modifier in DecoratorModifierParsers.FieldType.Token()
			from csharpType in CSharpTypes.CSharpType.Token()
			from name in CoreParser.Identifier.Token()
			select new DecoratorField
			{
				Index = number,
				Type = csharpType,
				Modifier = Modifier,
				Name = name
			};

		public static readonly Parser<string[]> IdentifierList =
			from identifiers in CoreParser.Identifier.DelimitedBy(Parse.Char(',').Token())
			select identifiers.ToArray();

		public static readonly Parser<string[]> InheritList =
			from _ in Parse.Char('[').Once()
			from identifiers in IdentifierList
			from __ in Parse.Char(']').Once()
			select identifiers;

		public static readonly Parser<DecoratorClass> DecoratorClassHeader =
			from name in CoreParser.Identifier.Token()
			from inherits in InheritList.Optional()
			from _ in Parse.Char(':').Token()
			select new DecoratorClass
			{
				Name = name,
				Inherits = inherits.IsEmpty ? new string[] { } : inherits.Get()
			};

		public static readonly Parser<DecoratorClass> DecoratorClass =
			from header in DecoratorClassHeader
			from fields in DecoratorField.Many()
			select new DecoratorClass
			{
				Name = header.Name,
				RawName = header.Name,
				Inherits = header.Inherits,
				Fields = fields.ToArray()
			};
	}
}