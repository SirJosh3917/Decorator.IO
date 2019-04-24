﻿using Sprache;
using System;
using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.Parser
{
	public static partial class LanguageParsers
	{
		public static readonly Parser<Type> CSharpInt =
			from _ in Parse.String("INT")
				.Or(Parse.String("I"))
			select typeof(int);

		public static readonly Parser<Type> CSharpUInt =
			from _ in Parse.String("UINT")
				.Or(Parse.String("UI"))
			select typeof(uint);

		public static readonly Parser<Type> CSharpString =
			from _ in Parse.String("STRING")
				.Or(Parse.String("STR"))
				.Or(Parse.String("S"))
			select typeof(string);

		public static readonly Parser<Type> CSharpType =
			from type in CSharpInt
				.Or(CSharpString)
				.Or(CSharpUInt)
			select type;
	}
}