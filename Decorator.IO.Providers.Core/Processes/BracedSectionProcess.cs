using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.Providers.Core.Processes
{
	public class BracedSectionProcess : IStringProcess
	{
		private readonly BracesProcess _bracesProcess;
		private readonly TabProcess _tabProcess;

		public BracedSectionProcess(BracesProcess bracesProcess, TabProcess tabProcess)
		{
			_bracesProcess = bracesProcess;
			_tabProcess = tabProcess;
		}

		public IEnumerable<StringBuilder> Process(IEnumerable<StringBuilder> enumerable)
			=> _bracesProcess.Process
			(
				_tabProcess.Process
				(
					enumerable
				)
			);
	}
}