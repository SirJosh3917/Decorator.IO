using Sprache;

using System;
using System.Linq;

namespace Decorator.IO.Parser
{
	public static class CoreParser
	{
		public static readonly Parser<int> NumberParser =
			from number in Parse.Digit.AtLeastOnce().Text()
			select int.Parse(number);

		public static readonly Parser<string> Namespace =
			from _ in Parse.String("NAMESPACE").Once().Token()
			from name in Parse.AnyChar.Until(Parse.Char(';')).Text()
			select name;

		public static readonly Parser<string> Identifier =
			from chars in Parse.Chars("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_1234567890")
				.AtLeastOnce().Token()
			select new string(chars.ToArray());

		public static readonly Parser<int> DecoratorNumber =
			from _ in Parse.Char('(').Optional().Token()
			from number in NumberParser.Token()
			from __ in Parse.Char(')').Optional().Token()
			select number;
	}
}