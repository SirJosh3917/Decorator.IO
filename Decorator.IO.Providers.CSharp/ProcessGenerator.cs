using Decorator.IO.Providers.Core.Processes;

namespace Decorator.IO.Providers.CSharp
{
	public static class ProcessGenerator
	{
		public static BracedSectionProcess NewBracedSectionProcess()
			=> new BracedSectionProcess(NewBracesProcess(), NewTabProcess());

		public static BracesProcess NewBracesProcess()
			=> new BracesProcess();

		public static TabProcess NewTabProcess()
			=> new TabProcess(tabAmount: 1);
	}
}