using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Decorator.IO.Core;
using Decorator.IO.Core.Tokens;
using Humanizer;

namespace Decorator.IO.Providers.CSharp
{
	public class CSharpProvider : ILanguageProvider
	{
		public byte[] Generate(Namespace ns)
		{
			using (var ms = new MemoryStream())
			using (var sw = new StreamWriter(ms))
			{
				sw.AutoFlush = false;

				WriteModels(sw, ns);

				sw.Flush();

				return ms.ToArray();
			}
		}

		private static void WriteModels(StreamWriter sw, Namespace ns)
		{
			sw.WriteLine($"namespace {ns.Name}");
			sw.WriteLine("{");

			foreach(var str in WriteModels(ns.Models))
			{
				sw.WriteLine($"\t{str}");
			}

			sw.WriteLine("}");
		}

		private static IEnumerable<string> WriteModels(Model[] models)
		{
			return models.SelectMany(x => WriteModel(x));
		}

		private static IEnumerable<string> WriteModel(Model model)
		{
			yield return $"public class {model.Identifier.Pascalize()}";

			if (model.Parents.Length > 0)
			{
				yield return ":";
				yield return model.Parents.Select(x => x.Model.Identifier.Pascalize()).Aggregate((a, b) => $"{a}, {b}");
			}

			yield return "{";
			yield return "}";
		}
	}
}
