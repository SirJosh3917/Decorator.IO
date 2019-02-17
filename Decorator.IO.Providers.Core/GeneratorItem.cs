using System.Text;

namespace Decorator.IO.Providers.Core
{
	public class GeneratorItem
	{
		public GeneratorItem(string str)
			=> StringBuilder = new StringBuilder(str);

		public GeneratorItem(StringBuilder stringBuilder)
			=> StringBuilder = stringBuilder;

		public StringBuilder StringBuilder { get; }

		public static implicit operator GeneratorItem(StringBuilder strb)
			=> new GeneratorItem(strb);

		public static implicit operator GeneratorItem(string str)
			=> new GeneratorItem(str);

		public static implicit operator StringBuilder(GeneratorItem generatorItem)
			=> generatorItem.StringBuilder;
	}
}
