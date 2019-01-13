using System.Collections.Generic;

namespace Decorator.IO
{
	public interface ICodeGenerator
	{
		void WorkOn(IEnumerable<Message> message);

		string Generate();
	}
}