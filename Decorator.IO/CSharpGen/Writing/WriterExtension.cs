using System.Collections.Generic;

namespace Decorator.IO.CSharpGen.Writer
{
	public abstract class WriterExtension : IWriter
	{
		protected readonly IWriter _writer;

		protected WriterExtension(IWriter writer) => _writer = writer;

		public List<string> Lines { get; } = new List<string>();
		private string _appendCache = "";

		public virtual void Append(string str) => _appendCache += str;

		public virtual void AppendLine() => AppendLine("");

		public virtual void AppendLine(string str)
		{
			Lines.Add($"{_appendCache}{str}");

			if (_appendCache.Length > 0)
			{
				_appendCache = "";
			}
		}

		public abstract IEnumerable<string> Write();

		public virtual void Dispose()
		{
			foreach (var line in Write())
			{
				_writer.AppendLine(line);
			}
		}
	}
}