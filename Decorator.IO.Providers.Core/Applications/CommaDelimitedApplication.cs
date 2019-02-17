using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.Providers.Core.Applications
{
	public class CommaDelimitedApplication : IStringBuilderApplication
	{
		private DelimitedApplication _delimiter;

		public CommaDelimitedApplication(IEnumerable<string> values)
			=> _delimiter = new DelimitedApplication(values, ", ");

		public void Apply(StringBuilder item)
			=> _delimiter.Apply(item);
	}
}