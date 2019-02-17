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

			strb.Append(" : IModel<");
			strb.Append(input.ClassName);
			strb.Append(">");

			foreach(var inherit in input.Inherits)
			{
				strb.Append(", ");
				strb.Append(inherit);
			}


			yield return strb.ToString();
		}
	}
}
