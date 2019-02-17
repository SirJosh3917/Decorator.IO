using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.Providers.Core.Processes
{
	public class TabProcess : IStringProcess
	{
		private readonly PrefixProcess _prefixPostProcess;

		public TabProcess(int tabAmount)
			=> _prefixPostProcess = new PrefixProcess(new string('\t', tabAmount));

		public IEnumerable<StringBuilder> Process(IEnumerable<StringBuilder> enumerable)
			=> _prefixPostProcess.Process(enumerable);
	}
}