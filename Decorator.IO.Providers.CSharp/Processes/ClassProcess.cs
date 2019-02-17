using Decorator.IO.Providers.Core;
using Decorator.IO.Providers.Core.Applications;

using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.Providers.CSharp.Processes
{
	public class ClassProcess : IStringProcess
	{
		private readonly string _name;
		private readonly string _modifiers;
		private readonly string[] _inherit;

		public ClassProcess
		(
			string name,
			string modifiers,
			string[] inherit
		)
		{
			_name = name;
			_modifiers = modifiers;
			_inherit = inherit;
		}

		public IEnumerable<StringBuilder> Process(IEnumerable<StringBuilder> enumerable)
		{
			var strb = new StringBuilder(_modifiers);
			strb.Append(" ");
			strb.Append(_name);

			if (_inherit.Length > 0)
			{
				strb.Append(" : ");

				new CommaDelimitedApplication(_inherit)
					.Apply(strb);
			}

			yield return strb;

			var bracedSectionProcess = ProcessGenerator.NewBracedSectionProcess();

			foreach (var line in bracedSectionProcess.Process(enumerable))
			{
				yield return line;
			}
		}
	}
}