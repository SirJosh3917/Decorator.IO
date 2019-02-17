using Decorator.IO.Core.Tokens;
using Decorator.IO.Providers.Core;
using Decorator.IO.Providers.CSharp.Processes;

using Humanizer;

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decorator.IO.Providers.CSharp
{
	public class CSharpProvider : LanguageProvider
	{
		public override string ModifyStringCasing(string str) => str.Pascalize();

		public override IEnumerable<StringBuilder> GenerateFrom(Namespace dioNamespace)
		{
			var nsProces = new NamespaceProcess(dioNamespace.Name);

			var classProcesses =
				dioNamespace
				.Models
				.SelectMany(model => new ClassProcess[]
				{
					new ClassProcess
					(
						name: $"I{model.Identifier}",
						modifiers: "public interface",
						inherit: model.Parents.Select(x => $"I{x.Model.Identifier}")
						.Prepend($"IModel<I{model.Identifier}>").ToArray()
					)
				});

			return nsProces.Process(classProcesses.SelectMany(x => x.Process(new StringBuilder[] { })));
		}
	}
}