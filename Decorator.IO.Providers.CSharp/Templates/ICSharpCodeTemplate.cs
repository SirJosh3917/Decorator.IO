using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decorator.IO.Providers.CSharp
{
	public interface ICSharpCodeTemplate<TInput>
	{
		IEnumerable<string> Generate(TInput input);
	}
}
