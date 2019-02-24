using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Decorator.IO.Providers.CSharp
{
	public class ModelNonGenericInterface : IRoslynCodeProvider
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

	public class ModelInterface : IRoslynCodeProvider
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