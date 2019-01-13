using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.CSharpGen.Writer
{
	public class Writer : IWriter
	{
		public Writer()
		{
		}

		public Writer(StringBuilder stringBuilder) => StringBuilder = stringBuilder;

		public StringBuilder StringBuilder { get; } = new StringBuilder();

		public void Append(string str) => StringBuilder.Append(str);

		public void AppendLine(string str) => StringBuilder.AppendLine(str);

		public void AppendLine() => StringBuilder.AppendLine();

		public void Dispose()
		{
		}

		public IEnumerable<string> Write()
		{
			var str = StringBuilder.ToString();

			if (!str.Contains('\n'))
			{
				yield return str;
				yield break;
			}

			foreach (var line in str.Split('\n'))
			{
				yield return line;
			}
		}
	}
}