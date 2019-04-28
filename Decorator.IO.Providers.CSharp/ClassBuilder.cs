using Decorator.IO.Core;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.Providers.CSharp
{
	public static class ClassBuilder
	{
		private static IEqualityComparer<DecoratorField> _equalityComparer = new DummyEqualityComparer();

		private class DummyEqualityComparer : IEqualityComparer<DecoratorField>
		{
			public bool Equals(DecoratorField x, DecoratorField y)
				=> x.Name == y.Name;

			public int GetHashCode(DecoratorField obj) => obj.Name.GetHashCode();
		}

		public static IEnumerable<MemberDeclarationSyntax> BuildClass(DecoratorClass decoratorClass)
			=> $@"public class {decoratorClass.Name} : I{decoratorClass.Name}
{{
	{(ConcatenateFieldsOfParents(new[] { decoratorClass })).ToPropertyStrings(true)}
	public object[] Serialize()
	{{
		return DecoratorObject.Serialize(this);
	}}
}}"
			.AsCompilationUnitSyntax()
			.AsMemberDeclarationSyntaxes();

		private static DecoratorField[] ConcatenateFieldsOfParents(IEnumerable<DecoratorClass> decoratorClasses)
			=> decoratorClasses.SelectMany(x => x.Fields)
			.Concat(decoratorClasses.Select(x => x.Parents).SelectMany(ConcatenateFieldsOfParents))
			.Distinct(_equalityComparer)
			.ToArray();
	}
}