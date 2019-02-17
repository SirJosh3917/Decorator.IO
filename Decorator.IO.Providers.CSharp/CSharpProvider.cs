using Decorator.IO.Core;
using Decorator.IO.Core.Tokens;
using Decorator.IO.Providers.CSharp.Templates;
using Humanizer;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Decorator.IO.Providers.CSharp
{
	public class CSharpProvider : ILanguageProvider
	{
		public string ModifyStringCasing(string str) => str.Pascalize();

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

			foreach (var str in WriteModels(ns.Models))
			{
				sw.WriteLine($"\t{str}");
			}

			sw.WriteLine("}");
		}

		private static IEnumerable<string> WriteModels(Model[] models)
			=> models.SelectMany(x => WriteModel(x));

		private static IEnumerable<string> WriteModel(Model model)
		{
			return new ClassGeneratorTemplate().Generate(model);
		}
	}
}