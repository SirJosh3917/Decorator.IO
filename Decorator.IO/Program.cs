using Decorator.IO.Core;
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

				visitor.Visit(parser.models());

				var models = visitor.GetModels();

				ILanguageProvider csprovider = new CSharpProvider();
				var generated = csprovider.Generate(models);

				foreach (var b in generated)
				{
					Console.Write(Convert.ToChar(b));
				}

				System.Console.ReadLine();
			}
		}
	}
}