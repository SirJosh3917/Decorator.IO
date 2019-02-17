using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.Providers.Core.Applications
{
	public class DelimitedApplication : IStringBuilderApplication
	{
		private readonly IEnumerable<string> _values;
		private readonly string _appendInbetween;

		public DelimitedApplication(IEnumerable<string> values, string appendInbetween)
		{
			_values = values;
			_appendInbetween = appendInbetween;
		}

		public void Apply(StringBuilder item)
		{
			var enumerator = _values.GetEnumerator();

			if (!enumerator.MoveNext()) return;

			item.Append(enumerator.Current);

			while (enumerator.MoveNext())
			{
				item.Append(_appendInbetween);
				item.Append(enumerator.Current);
			}
		}
	}
}