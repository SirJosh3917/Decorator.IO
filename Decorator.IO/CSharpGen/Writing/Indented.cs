using System.Collections.Generic;

namespace Decorator.IO.CSharpGen.Writer
{
	public class Indented : WriterExtension
	{
		public Indented(IWriter writer) : base(writer)
		{
		}

		public override IEnumerable<string> Write()
		{
			foreach (var i in Lines)
			{
				yield return $"\t{i}";
			}
		}
	}
}