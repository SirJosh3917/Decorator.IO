using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Decorator.IO
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			lastWL = DateTime.Now;
			WriteLine($"Decorator.IO, at your service!", ConsoleColor.White);
#if DEBUG
			args = "--gen cs -i messages.json -o out.cs".Split(' ');
#endif

			var aargs = Terminal.ArgParser.Parse(args);

			WriteLine("Parsing args", ConsoleColor.Cyan);

			var genSelected = GetGen(aargs.GeneratorType, aargs.Namespace).GetType().ToString();

			Write("Using Generator ", ConsoleColor.DarkCyan);
			WriteLine(genSelected, ConsoleColor.Red);

			var data = aargs.InFilesAndOutFile.Select(x => x.Key).SelectMany(x => x).Select(x => File.ReadAllBytes(x)).ToArray();
			WriteLine("Reading raw data", ConsoleColor.DarkCyan);

			var inFiles =
				data.Select(x => Utf8Json.JsonSerializer.Deserialize<Message[]>(x))
				.ToArray();

			WriteLine("Deserializing to json", ConsoleColor.DarkCyan);

			var allMsgs = inFiles
				.SelectMany(x => x)
				.ToArray();

			allMsgs = allMsgs
				.Select(x => x.Clone())
				.Select(x => { x.Elements = x.InheritBase(allMsgs); return x; })
				.ToArray();

			WriteLine("Finish JSON-related computation", ConsoleColor.DarkCyan);

			var outItems = aargs.InFilesAndOutFile.Select(x => x.Value).Select(x => (IEnumerable<string>)new string[1] { x }).Aggregate((a, b) => a.Concat(b)).ToArray();
			var genType = aargs.GeneratorType;
			var @namespace = aargs.Namespace;
			if (outItems.Length == 1)
			{
				Gen(allMsgs, outItems[0], GetGen(genType, @namespace));
			}
			else
			{
				for (int i = 0; i < inFiles.Length; i++)
				{
					Gen(inFiles[i], outItems[i], GetGen(genType, @namespace));
				}
			}

			Console.ReadLine();
		}

		public static void Gen(Message[] messages, string outFile, ICodeGenerator codeGen)
		{
			Write("Outting to ", ConsoleColor.DarkCyan);
			WriteLine(outFile, ConsoleColor.Red);

			codeGen.WorkOn(messages);
			WriteLine("Working on generation", ConsoleColor.DarkMagenta);

			File.WriteAllText(outFile, codeGen.Generate());
			WriteLine("Writing", ConsoleColor.Green);
		}

		private static ICodeGenerator GetGen(string inGen, string ns)
		{
			switch (inGen)
			{
				case "cs":
				{
					return new CSharpGen.CSharpGenerator(ns);
				}

				default: throw new ArgumentException($"Unknown generator {inGen}");
			}
		}

		private static DateTime lastWL;

		private static void Write(string text, ConsoleColor color)
		{
			var curCol = Console.ForegroundColor;
			Console.ForegroundColor = color;
			Console.Write(text);
			Console.ForegroundColor = curCol;
		}

		private static void WriteLine(string text, ConsoleColor color)
		{
			var curCol = Console.ForegroundColor;
			Console.ForegroundColor = color;
			Console.Write($"{text} ");

			Console.ForegroundColor = ConsoleColor.Magenta;
			var curTime = DateTime.Now;
			Console.WriteLine($"{(curTime - lastWL).TotalMilliseconds}ms");
			lastWL = curTime;

			Console.ForegroundColor = curCol;
		}
	}
}