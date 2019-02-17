using Decorator.IO.Core.Tokens;
using Decorator.IO.Providers.Core;

using Humanizer;

using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.Providers.CSharp
{
	public class CSharpProvider : LanguageProvider
	{
		public override string ModifyStringCasing(string str) => str.Pascalize();

		public override IEnumerable<StringBuilder> GenerateFrom(Namespace dioNamespace)
		{
			yield break;
		}
	}
}