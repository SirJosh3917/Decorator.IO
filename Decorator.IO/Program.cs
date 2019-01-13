using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Decorator.IO
{
	public enum ReadState
	{
		None,
		Input,
		Output,
		Namespace,
		Generator
	}

	internal class Program
	{
		private static void Main(string[] args)
		{
			lastWL = DateTime.Now;
			WriteLine($"Decorator.IO, at your service!", ConsoleColor.White);
#if DEBUG
			args = "--gen cs -i messages.json -o out.cs".Split(' ');
#endif

			var inItems = new List<string>();
			var outItems = new List<string>();

			var @namespace = "Decorator.IO";
			string genType = "";

			var rs = ReadState.None;

			foreach (var i in args)
			{
				if (i == "-i")
				{
					rs = ReadState.Input;
					continue;
				}

				if (i == "-o")
				{
					rs = ReadState.Output;
					continue;
				}

				if (i == "--ns")
				{
					rs = ReadState.Namespace;
					continue;
				}

				if (i == "--gen")
				{
					rs = ReadState.Generator;
					continue;
				}

				switch (rs)
				{
					case ReadState.None: throw new ArgumentException($"Please specify -i <the files> and -o <the files>");
					case ReadState.Namespace: @namespace = i; break;
					case ReadState.Generator: genType = i; break;
					case ReadState.Input: inItems.Add(i); break;
					case ReadState.Output: outItems.Add(i); break;

					default: throw new ArgumentException("Unsupported operation");
				}
			}

			WriteLine("Parsing args", ConsoleColor.Cyan);

			var genSelected = GetGen(genType, @namespace).GetType().ToString();

			Write("Using Generator ", ConsoleColor.DarkCyan);
			WriteLine(genSelected, ConsoleColor.Red);

			var data = inItems.Select(x => File.ReadAllBytes(x)).ToArray();
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

			if (outItems.Count == 1)
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