using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.Providers.Core.Processes
{
	public class BracesProcess : IStringProcess
	{
		public IEnumerable<StringBuilder> Process(IEnumerable<StringBuilder> enumerable)
		{
			yield return new StringBuilder("{");

			var enumerator = enumerable.GetEnumerator();

			while (enumerator.MoveNext())
			{
				yield return enumerator.Current;
			}

			yield return new StringBuilder("}");
		}
	}
}