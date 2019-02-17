using Decorator.IO.Providers.Core;
using Decorator.IO.Providers.Core.Applications;

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decorator.IO.Providers.CSharp.Processes
{
	// TODO: _isInterface bool is a codesmell
	public class ClassProcess : IStringProcess
	{
		private readonly string _name;
		private readonly string _modifiers;
		private readonly IEnumerable<string> _inherit;
		private readonly bool _isInterface;

		public ClassProcess
		(
			string name,
			string modifiers,
			IEnumerable<string> inherit,
			bool isInterface = false
		)
		{
			_name = name;
			_modifiers = modifiers;
			_inherit = inherit;
			_isInterface = isInterface;
		}

		public IEnumerable<StringBuilder> Process(IEnumerable<StringBuilder> enumerable)
		{
			var strb = new StringBuilder(_modifiers);
			strb.Append(" ");

			if (_isInterface)
			{
				strb.Append("interface I");
			}
			else
			{
				strb.Append("class ");
			}

			strb.Append(_name);

			if (_inherit.Any())
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