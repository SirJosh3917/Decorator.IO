using Decorator.IO.Core;
using Decorator.IO.Core.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decorator.IO.Providers.CSharp.Templates
{
	public class ClassGeneratorTemplate : ICSharpCodeTemplate<Model>
	{
		public IEnumerable<string> Generate(Model input)
		{
			var cdt = new ClassDefinitionTemplate();

			var classDef = cdt.Generate(new ClassDefinitionArgs
			{
				ClassName = input.Identifier,
				Inherits = input.Parents.Select(x => x.Model.Identifier)
			});

			var mgt = new MethodGeneratorTemplate();

			var methodOne = mgt.Generate(new MethodGeneratorArgs
			{
				Name = "TestMethod",
				Static = false,
				Return = new VoidType(),
				Implementation = new string[0],
				Arguments = new string[] { "string a", "int b" }
			});

			var pdt = new PropertyDefinitionTemplate();

			var properties = new List<string>();

			foreach(var field in input.Fields)
			{
				properties.AddRange(pdt.Generate(new PropertyDefinitionArgs
				{
					Identifier = field.Identifier,
					IType = field.Type
				}));
			}

			return classDef
				.Append("{")
				.Concat(properties.Select(x => $"\t{x}"))
				.Concat(methodOne.Select(x => $"\t{x}"))
				.Append("}");
		}
	}
}
