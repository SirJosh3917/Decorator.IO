using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InOutKVP = System.Collections.Generic.KeyValuePair<System.Collections.Generic.IEnumerable<string>, string>;

namespace Decorator.IO.T
{
	public class ArgParseResults
	{
		public IEnumerable<InOutKVP> InFilesAndOutFile { get; set; }
		public string Namespace { get; set; }
		public string GeneratorType { get; set; }
	}

	public enum ReadState
	{
		None,
		Input,
		Output,
		Namespace,
		Generator
	}

	public static class ArgParser
	{
		public static ArgParseResults Parse(IEnumerable<string> args)
		{
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
					case ReadState.Namespace: @namespace = i; break;
					case ReadState.Generator: genType = i; break;
					case ReadState.Input: inItems.Add(i); break;
					case ReadState.Output: outItems.Add(i); break;

					case ReadState.None: throw new ArgumentException($"Please specify -i <the files> and -o <the files>");
					default: throw new ArgumentException("Unsupported operation");
				}
			}

			return new ArgParseResults
			{
				InFilesAndOutFile = Intermingle(inItems, outItems),
				GeneratorType = genType,
				Namespace = @namespace
			};
		}

		public static IEnumerable<InOutKVP> Intermingle(IEnumerable<string> inFiles, IEnumerable<string> outFiles)
		{
			if (outFiles.Count() == 1)
			{
				yield return new InOutKVP(inFiles, outFiles.Single());
			}
			else
			{
				foreach(var i in inFiles.Zip(outFiles, (a, b) => (a, b)))
				{
					yield return new InOutKVP(new string[] { i.a }, i.b);
				}
			}
		}
	}
}
