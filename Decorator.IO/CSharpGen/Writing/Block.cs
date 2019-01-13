using System.Collections.Generic;

namespace Decorator.IO.CSharpGen.Writer
{
	public class Block : WriterExtension
	{
		public Block(IWriter writer) : base(writer)
		{
		}

		public override IEnumerable<string> Write()
		{
			yield return "{";

			using (var indented = new Indented(_writer))
			{
				foreach (var line in Lines)
				{
					indented.AppendLine(line);
				}
			}

			yield return "}";
		}
	}
}