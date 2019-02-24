using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Decorator.IO.Providers.CSharp
{
	public class IModelNonGenericInterface : IRoslynCodeProvider
	{
		public MemberDeclarationSyntax Provide()
		{
			var options = new CSharpParseOptions(LanguageVersion.Latest);
			return CSharpSyntaxTree.ParseText(@"
public interface IModel
{
	object[] Serialize();
}
", options).GetCompilationUnitRoot().Members.First();
		}
	}

	public class IModelProvider : IRoslynCodeProvider
	{
		public MemberDeclarationSyntax Provide()
		{
			var options = new CSharpParseOptions(LanguageVersion.Latest);
			return CSharpSyntaxTree.ParseText(@"
public interface IModel<T> : IModel
{
}
", options).GetCompilationUnitRoot().Members.First();
		}
	}
}