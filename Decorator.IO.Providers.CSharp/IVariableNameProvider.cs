using System;
using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.Providers.CSharp
{
	public interface IVariableNameProvider
	{
		string New();
	}

	public class DefaultVariableNameProvider : IVariableNameProvider
	{
		private int _start;

		public DefaultVariableNameProvider(int start = 0) => _start = start;

		public string New() => $"_{(_start++).ToString()}";
	}
}
