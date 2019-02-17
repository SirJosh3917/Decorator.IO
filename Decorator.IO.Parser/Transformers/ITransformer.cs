namespace Decorator.IO.Parser
{
	public interface ITransformer<TInput, TOutput>
	{
		TOutput Transform(TInput input);
	}
}