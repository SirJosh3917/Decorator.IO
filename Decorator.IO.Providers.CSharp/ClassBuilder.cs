using Decorator.IO.Core;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;

namespace Decorator.IO.Providers.CSharp
{
	public static class ClassBuilder
	{
		public static IEnumerable<MemberDeclarationSyntax> BuildClass(DecoratorClass decoratorClass)
			=> $@"public class {decoratorClass.Name} : I{decoratorClass.Name}
{{
	{decoratorClass.ConcatenateFieldsOfParents().ToPropertyStrings(true)}
	public object[] Serialize()
	{{
		return this.SerializeAs{decoratorClass.Name}();
	}}
}}"
			.AsCompilationUnitSyntax()
			.AsMemberDeclarationSyntaxes();
	}
}