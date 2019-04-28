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
	public object[] {Config.InterfaceSerializeName}()
	{{
		return this.{Config.SerializeAsName(decoratorClass.Name)}();
	}}
}}"
			.AsCompilationUnitSyntax()
			.AsMemberDeclarationSyntaxes();
	}
}