using Decorator.IO.Core;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;
using System.Linq;

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
		return DecoratorObject.Serialize(this);
	}}
}}"
			.AsCompilationUnitSyntax()
			.AsMemberDeclarationSyntaxes();
	}
}