using Decorator.IO.Core;

using Sprache;

using System;

namespace Decorator.IO.Parser
{
	public static class CSharpTypes
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

		public static readonly Parser<Type> CSharpFloat =
			from _ in Parse.String("FLOAT")
				.Or(Parse.String("FLT"))
				.Or(Parse.String("F"))
			select typeof(float);

		public static readonly Parser<Type> CSharpBoolean =
			from _ in Parse.String("BOOLEAN")
				.Or(Parse.String("BOOL"))
				.Or(Parse.String("B"))
			select typeof(bool);

		public static readonly Parser<Type> CSharpDouble =
			from _ in Parse.String("DOUBLE")
				.Or(Parse.String("DBL"))
			select typeof(double);

		public static readonly Parser<Type> CSharpDecimal =
			from _ in Parse.String("DECIMAL")
				.Or(Parse.String("DEC"))
			select typeof(decimal);

		public static readonly Parser<DummyType> Any =
			from name in CoreParser.Identifier
			select new DummyType(name);

		public static readonly Parser<Type> CSharpType =
			from type in CSharpInt
				.Or(CSharpString)
				.Or(CSharpUInt)
				.Or(CSharpFloat)
				.Or(CSharpBoolean)
				.Or(CSharpDouble)
				.Or(CSharpDecimal)

				.Or(Any)
			select type;
	}
}