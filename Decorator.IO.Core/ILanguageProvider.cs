using Decorator.IO.Core.Tokens;

namespace Decorator.IO.Core
{
	public interface ILanguageProvider
	{
		string ModifyStringCasing(string str);

		byte[] Generate(Namespace ns);
	}
}