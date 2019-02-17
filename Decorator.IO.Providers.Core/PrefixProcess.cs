using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.Providers.Core
{
	public class PrefixProcess : IStringProcess
	{
		private readonly string _prefix;

		public PrefixProcess(string prefix) => _prefix = prefix;

		public IEnumerable<StringBuilder> Process(IEnumerable<StringBuilder> enumerable)
		{
			foreach (var strb in enumerable)
			{
				strb.Insert(0, _prefix);

				yield return strb;
			}
		}
	}
}