using Decorator.IO.Core.Tokens;

namespace Decorator.IO.Core
{
	public interface ILanguageProvider
	{
		byte[] Generate(Namespace ns);
	}
}