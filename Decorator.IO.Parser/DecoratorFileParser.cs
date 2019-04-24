using Sprache;

using System.Linq;

namespace Decorator.IO.Parser
{
	public static class DecoratorFileParser
	{
		public static readonly Parser<DecoratorFile> FileParser =
			from ns in CoreParser.Namespace.Token()
			from classes in DecoratorPocoParser.DecoratorClass.Many()
			select new DecoratorFile
			{
				Namespace = ns,
				Classes = classes.ToArray()
			};
	}
}