using Decorator.IO.Core;
using Decorator.IO.Core.Tokens;

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Decorator.IO.Providers.Core
{
	public abstract class LanguageProvider : ILanguageProvider
	{
		public abstract string ModifyStringCasing(string str);

		public abstract IEnumerable<StringBuilder> GenerateFrom(Namespace dioNamespace);

		public byte[] Generate(Namespace ns)
		{
			using (var ms = new MemoryStream())
			using (var sw = new StreamWriter(ms))
			{
				sw.AutoFlush = false;

				foreach (var stringbuilder in GenerateFrom(ns))
				{
					sw.WriteLine(stringbuilder.ToString());
				}

				sw.Flush();

				return ms.ToArray();
			}
		}
	}
}