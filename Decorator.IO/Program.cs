using Decorator.IO.T;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InOutKVP = System.Collections.Generic.KeyValuePair<System.Collections.Generic.IEnumerable<string>, string>;
using SConsole = System.Console;
using ConsoleColor = System.ConsoleColor;
using System;

namespace Decorator.IO
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			using (var ta = new TimedAction("Decorator.IO, at your service!", ConsoleColor.White))
			{
			}
#if DEBUG
			args = "--gen cs -i messages.json -o out.cs".Split(' ');
#endif
			ArgParseResults aargs;

			aargs = ParseArgs(args);

			DetermineGenerator(aargs, out var generator, out var genName);

			var readingData = ReadData(aargs);

			var deserializedData = DeserializeData(readingData);

			GenerateFiles(generator, deserializedData);

			SConsole.ReadLine();
		}

		private static ArgParseResults ParseArgs(string[] args)
		{
			ArgParseResults aargs;
			using (var ta = new TimedAction("Parsing arguments", ConsoleColor.Cyan))
			{
				aargs = ArgParser.Parse(args);
			}

			return aargs;
		}

		private static void DetermineGenerator(ArgParseResults aargs, out Func<ICodeGenerator> generator, out string genName)
		{
			using (var ta = new TimedAction("Determining generator: ", ConsoleColor.DarkCyan))
			{
				generator = () => GetGen(aargs.GeneratorType, aargs.Namespace);
				genName = (generator()).GetType().ToString();

				Terminal.Write($"{genName}", ta.Color);
			}
		}

		private static KeyValuePair<IEnumerable<byte[]>, string>[] ReadData(ArgParseResults aargs)
		{
			KeyValuePair<IEnumerable<byte[]>, string>[] readingData;
			using (var ta = new TimedAction("Reading in data", ConsoleColor.DarkCyan))
			{
				readingData =
					aargs.InFilesAndOutFile
					.Select(x => new KeyValuePair<IEnumerable<byte[]>, string>
						(
							x.Key.Select(xy => File.ReadAllBytes(xy)),
							x.Value
						))
					.ToArray();
			}

			return readingData;
		}

		private static KeyValuePair<Message[], string>[] DeserializeData(KeyValuePair<IEnumerable<byte[]>, string>[] readingData)
		{
			KeyValuePair<Message[], string>[] deserializedData;
			using (var ta = new TimedAction("Deserializing data", ConsoleColor.DarkCyan))
			{
				deserializedData =
					readingData
					.Select(x => new KeyValuePair<Message[], string>
					(
						x.Key.Select(y => Utf8Json.JsonSerializer.Deserialize<Message[]>(y)).SelectMany(y => y).ToArray(),
						x.Value
					))
					.ToArray();
			}

			return deserializedData;
		}

		private static void GenerateFiles(Func<ICodeGenerator> generator, KeyValuePair<Message[], string>[] deserializedData)
		{
			foreach (var element in deserializedData)
			{
				Generate(element.Key, element.Value, generator());
			}
		}
		private static void Generate(Message[] messages, string file, ICodeGenerator codeGen)
		{
			string result;

			using (var ta = new TimedAction("Generating", ConsoleColor.DarkCyan))
			{
				codeGen.WorkOn(messages);
				result = codeGen.Generate();
			}

			using (var ta = new TimedAction("Writing to file", ConsoleColor.DarkMagenta))
			{
				File.WriteAllText(file, result);
			}
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
	}
}