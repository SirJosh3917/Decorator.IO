using System.Text;

namespace Decorator.IO.CSharpGen.Writer
{
	public static class CSFileWriter
	{
		public static string Generate(this CSFile file)
		{
			var strb = new StringBuilder();

			foreach (var i in file.UsingNamespaces)
			{
				strb.AppendLine($"using {i}");
			}

			strb.AppendLine();

			// TODO: a prettier way of indenting \t automagically

			using (var writer = new Writer(strb))
			{
				foreach (var ns in file.Namespaces)
				{
					WriteNamespace(writer, ns);
				}
			}

			return strb.ToString();
		}

		private static void WriteNamespace(Writer writer, Namespace ns)
		{
			writer.AppendLine($"namespace {ns.Name}");

			bool lineRightAfter = true;

			using (var nsBlock = new Block(writer))
			{
				foreach (var c in ns.Classes)
				{
					if (!lineRightAfter)
					{
						nsBlock.AppendLine();
					}

					WriteClass(nsBlock, c);

					lineRightAfter = false;
				}
			}
		}

		private static void WriteClass(Block nsBlock, Class c)
		{
			nsBlock.AppendLine($"public class {c.Name}");

			using (var cBlock = new Block(nsBlock))
			{
				foreach (var prop in c.Properties)
				{
					WriteProperty(cBlock, prop);
				}

				foreach (var method in c.Methods)
				{
					cBlock.AppendLine();

					WriteMethodSignature(cBlock, method);

					WriteMethodsCode(cBlock, method);
				}
			}
		}

		private static void WriteProperty(Block cBlock, Property prop)
			=> cBlock.AppendLine($"public {prop.Type.ToString()}{(prop.IsArray ? "[]" : "")} {prop.Name} {{ get; set; }}");

		private static void WriteMethodSignature(Block cBlock, Method method)
		{
			foreach (var attribute in method.Attributes)
			{
				cBlock.AppendLine($"[{attribute}]");
			}

			cBlock.Append($"public {(method.IsStatic ? "static " : "")}{(method.IsOverride ? "override " : "")}{WriteShorter(method.ReturnType.ToString())} {method.Name}");

			WriteMethodParameters(cBlock, method);
		}

		private static void WriteMethodParameters(Block cBlock, Method method)
		{
			cBlock.Append("(");

			for (int i = 0; i < method.Parameters.Count; i++)
			{
				var param = method.Parameters[i];
				bool isNext = i < method.Parameters.Count - 1;

				WriteParameter(cBlock, param, isNext);
			}

			cBlock.AppendLine(")");
		}

		private static void WriteParameter(Block cBlock, MethodParameter param, bool isNext)
		{
			cBlock.Append($"{(param.IsRef ? "ref " : "")}{(param.IsOut ? "out " : "")}{param.StringParameterType} {param.Name}");

			if (isNext)
			{
				cBlock.Append(", ");
			}
		}

		private static void WriteMethodsCode(Block cBlock, Method method)
		{
			using (var mBlock = new Block(cBlock))
			{
				foreach (var line in method.Code)
				{
					mBlock.AppendLine(line);
				}
			}
		}

		private static string WriteShorter(string type)
		{
			switch (type)
			{
				case "System.Void": return "void";
				default: return type;
			}
		}
	}
}