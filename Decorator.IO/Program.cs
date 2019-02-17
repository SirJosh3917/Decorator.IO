using Decorator.IO.Core;
using Decorator.IO.Core.Tokens;
using Decorator.IO.Parser;
using Decorator.IO.Providers.CSharp;

using System;
using System.IO;

namespace Decorator.IO
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var file =
#if DEBUG
				"input.dio";
#else
				args[0];
#endif

			using (var fs = File.OpenRead(file))
			{
				var parser = GenParser.GetParser(new Antlr4.Runtime.AntlrInputStream(fs));
				var visitor = new Tokenizer();

				ILanguageProvider csprovider = new CSharpProvider();

				var ns = visitor.VisitModels(parser.models()) as Namespace;
				foreach (var model in ns.Models)
				{
					model.Identifier = csprovider.ModifyStringCasing(model.Identifier);

					foreach (var field in model.Fields)
					{
						field.Identifier = csprovider.ModifyStringCasing(field.Identifier);
					}
				}

				var generated = csprovider.Generate(ns);

				foreach (var b in generated)
				{
					Console.Write(Convert.ToChar(b));
				}

				File.WriteAllBytes("input.cs", generated);

				System.Console.ReadLine();
			}
		}
	}
}