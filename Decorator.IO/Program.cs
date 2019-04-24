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
			}
		}
	}
}