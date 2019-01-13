namespace Decorator.IO.CSharpGen.Interpreters
{
	internal interface IInterpreter<TIn, TOut>
	{
		TOut Interpet(TIn input);
	}
}