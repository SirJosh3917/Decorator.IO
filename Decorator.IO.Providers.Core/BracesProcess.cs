using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.Providers.Core
{
	public class BracesProcess : IStringProcess
	{
		private readonly bool _bracesOnNewline;

		public BracesProcess(bool bracesOnNewline = true) => _bracesOnNewline = bracesOnNewline;

		public IEnumerable<StringBuilder> Process(IEnumerable<StringBuilder> enumerable)
		{
			var enumerator = enumerable.GetEnumerator();

			if (!enumerator.MoveNext())
			{
				yield break;
			}

			if (!_bracesOnNewline)
			{
				enumerator.Current.Append(" {");
			}
			else
			{
				yield return new StringBuilder("{");
			}

			yield return enumerator.Current;

			while (enumerator.MoveNext())
			{
				yield return enumerator.Current;
			}

			yield return new StringBuilder("}");
		}
	}
}