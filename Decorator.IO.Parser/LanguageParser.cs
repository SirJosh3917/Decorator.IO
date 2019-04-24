using Sprache;
using System;
using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.Parser
{
	public class DecoratorFieldTypeParsers
	{
		public static readonly Parser<DecoratorType> Required =
			from @required in Parse.String("R")
				.Or(Parse.String("REQ"))
				.Or(Parse.String("REQUIRED"))
			select DecoratorType.Required;

		public static readonly Parser<DecoratorType> Optional =
			from @required in Parse.String("O")
				.Or(Parse.String("OPT"))
				.Or(Parse.String("OPTIONAL"))
			select DecoratorType.Required;
	}

	public static class LanguageParser
	{
		public static readonly Parser<string> Number
			= Parse.Digit.AtLeastOnce()
				.Text()
				.Token();

		public static readonly Parser<string> Namespace =
			from @namespace in Parse.String("NAMESPACE").Once()
			from @whitespace in Parse.WhiteSpace.AtLeastOnce()
			from name in Parse.AnyChar.Until(Parse.Char(';')).Text()
			select name;

		public static readonly Parser<DecoratorType> FieldType =
			from fieldType in DecoratorFieldTypeParsers.Required
				.Or(DecoratorFieldTypeParsers.Optional)
			select fieldType;

		public static readonly Parser<Type> CSharpType =
			null;

		public static readonly Parser<string> FieldName =
			null;

		public static readonly Parser<DecoratorField> DecoratorField =
			from @pipe in Parse.Char('|').Once()
			from @whitespace in Parse.WhiteSpace.Optional()
			from @openParenthesis in Parse.Char('(').Optional()
			from @whitespace1 in Parse.WhiteSpace.Optional()
			from number in Number
			from @whitespace2 in Parse.WhiteSpace.Optional()
			from @closeParenthesis in Parse.Char(')').Optional()
			from @whitespace3 in Parse.WhiteSpace.AtLeastOnce()
			from fieldType in FieldType
			from @whitespace4 in Parse.WhiteSpace.AtLeastOnce()
			from csharpType in CSharpType
			from @whitespace5 in Parse.WhiteSpace.AtLeastOnce()
			from name in FieldName
			select new DecoratorField
			{
				Index = int.Parse(number),
				Type = fieldType,
				CSharpType = csharpType,
				Name = name
			};
	}

	public class DecoratorField
	{
		public int Index { get; set; }
		public DecoratorType Type { get; set; }
		public Type CSharpType { get; set; }
		public string Name { get; set; }
	}

	public enum DecoratorType
	{
		Required,
		Optional
	}
}
