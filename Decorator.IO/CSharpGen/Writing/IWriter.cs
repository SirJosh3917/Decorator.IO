using System;
using System.Collections.Generic;

namespace Decorator.IO.CSharpGen.Writer
{
	public interface IWriter : IDisposable
	{
		void Append(string str);

		void AppendLine(string str);

		void AppendLine();

		IEnumerable<string> Write();
	}
}