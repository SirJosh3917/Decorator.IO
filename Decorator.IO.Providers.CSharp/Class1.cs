using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Decorator.IO.Core;
using Decorator.IO.Core.Tokens;

namespace Decorator.IO.Providers.CSharp
{
	public class CSharpProvider : ILanguageProvider
	{
		public byte[] Generate(Model[] models)
		{
			using (var ms = new MemoryStream())
			using (var sw = new StreamWriter(ms))
			{
				sw.AutoFlush = false;

				WriteModels(sw, models);

				sw.Flush();

				return ms.ToArray();
			}
		}

		private static void WriteModels(StreamWriter sw, Model[] models)
		{
			foreach(var str in WriteModels(models))
			{
				sw.WriteLine(str);
			}
		}

		private static IEnumerable<string> WriteModels(Model[] models)
		{
			return models.SelectMany(x => WriteModel(x));
		}

		private static IEnumerable<string> WriteModel(Model model)
		{
			yield return model.Identifier;
		}
	}
}
