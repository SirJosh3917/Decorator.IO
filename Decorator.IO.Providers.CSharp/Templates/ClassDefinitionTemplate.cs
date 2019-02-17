using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decorator.IO.Providers.CSharp.Templates
{
	public class ClassDefinitionArgs
	{
		public string ClassName { get; set; }
		public IEnumerable<string> Inherits { get; set; }
	}

	public class ClassDefinitionTemplate : ICSharpCodeTemplate<ClassDefinitionArgs>
	{
		public IEnumerable<string> Generate(ClassDefinitionArgs input)
		{
			var strb = new StringBuilder("public class ");

			strb.Append(input.ClassName);

			if (input.Inherits.Any())
			{
				strb.Append(" : ");

				var enumerator = input.Inherits.GetEnumerator();

				enumerator.MoveNext();
				strb.Append(enumerator.Current);

				while (enumerator.MoveNext())
				{
					strb.Append(", ");
					strb.Append(enumerator.Current);
				}
			}

			yield return strb.ToString();
		}
	}
}
