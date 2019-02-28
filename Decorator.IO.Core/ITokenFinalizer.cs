namespace Decorator.IO.Core
{
	public interface ITokenFinalizer
	{
		FinalizedTokens.Namespace Finalize(Tokens.Namespace ns);
	}
}