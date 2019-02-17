using Decorator.IO.Core.Tokens;
using Decorator.IO.Providers.Core;
using Decorator.IO.Providers.CSharp.Generators;
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

			var modelGen = new ModelsGenerator(dioNamespace.Models);

			return nsProces.Process(modelGen);
		}
	}
}