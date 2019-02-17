using Decorator.IO.Providers.Core;

using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.Providers.CSharp.Processes
{
	public class NamespaceProcess : IStringProcess
	{
		private readonly string _namespaceName;

		public NamespaceProcess(string namespaceName) => _namespaceName = namespaceName;

		public IEnumerable<StringBuilder> Process(IEnumerable<StringBuilder> enumerable)
		{
			var ns = new StringBuilder("namespace ");
			ns.Append(_namespaceName);

			yield return ns;

			var bracedSectionProcess = ProcessGenerator.NewBracedSectionProcess();

			foreach (var line in bracedSectionProcess.Process(enumerable))
			{
				yield return line;
			}
		}
	}
}