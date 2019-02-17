using Decorator.IO.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decorator.IO.Providers.CSharp.Templates
{
	public class MethodGeneratorArgs
	{
		public string Name { get; set; }
		public bool Static { get; set; }
		public IType Return { get; set; }

		public IEnumerable<string> Arguments { get; set; }
		public IEnumerable<string> Implementation { get; set; }
	}

	public class MethodGeneratorTemplate : ICSharpCodeTemplate<MethodGeneratorArgs>
	{
		public IEnumerable<string> Generate(MethodGeneratorArgs input)
		{
			var strb = new StringBuilder();
			strb.Append("public ");

			if (input.Static)
			{
				strb.Append("static ");
			}

			strb.Append(input.Return.Identifier);
			strb.Append(" ");
			strb.Append(input.Name);

			strb.Append("(");

			if (input.Arguments.Any())
			{
				var enumerator = input.Arguments.GetEnumerator();

				enumerator.MoveNext();
				strb.Append(enumerator.Current);

				while(enumerator.MoveNext())
				{
					strb.Append(", ");
					strb.Append(enumerator.Current);
				}
			}

			strb.Append(")");

			yield return strb.ToString();
			yield return "{";

			foreach(var line in input.Implementation)
			{
				yield return $"\t{line}";
			}

			yield return "}";
		}
	}
}
