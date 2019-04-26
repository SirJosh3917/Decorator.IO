using Decorator.IO.Core;
using Decorator.IO.Parser;
using Decorator.IO.Providers.CSharp;

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
			using (var sr = new StreamReader(fs))
			using (var outfs = File.OpenWrite("out.cs"))
			{
				IParser parser = new DecoratorIOParser();
				var result = parser.Parse(sr.ReadToEnd());

				IProvider provider = new CSharpProvider();
				provider.Provide(outfs, result);
			}
		}
	}
}