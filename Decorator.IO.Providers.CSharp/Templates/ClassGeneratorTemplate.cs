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
			var classDef = ClassDefinition(input);
			var properties = Properties(input.Fields);

			var mgt = new MethodGeneratorTemplate();

			var methodOne = mgt.Generate(new MethodGeneratorArgs
			{
				Name = "TestMethod",
				Static = false,
				Return = new VoidType(),
				Implementation = new string[0],
				Arguments = new string[] { "string a", "int b" }
			});

			return classDef
				.Append("{")
				.Concat
				(
					properties
					.Concat(methodOne)

					.Select(x => $"\t{x}")
				)
				.Append("}");
		}

		public IEnumerable<string> ClassDefinition(Model input)
		{
			var cdt = new ClassDefinitionTemplate();

			var classDef = cdt.Generate(new ClassDefinitionArgs
			{
				ClassName = input.Identifier,
				Inherits = input.Parents.Select(x => x.Model.Identifier)
			});

			return classDef;
		}

		public IEnumerable<string> Properties(IEnumerable<Field> fields)
		{
			var pdt = new PropertyDefinitionTemplate();

			foreach(var field in fields)
			{
				foreach(var line in pdt.Generate(new PropertyDefinitionArgs
				{
					Identifier = field.Identifier,
					Type = field.Type
				}))
				{
					yield return line;
				}
			}
		}

		public IEnumerable<string> BaseInterface()
		{
			yield return "public interface IModel<T>";
			yield return "	where T : IModel<T>";
			yield return "{";
			yield return "	T Deserialize(object[] data);";
			yield return "	object[] Serialize(T instance);";
			yield return "}";
		}
	}
}
