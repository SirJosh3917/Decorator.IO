using Decorator.IO.Core;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.Providers.CSharp
{
	public class ClassBuilder
	{
		private class DummyEqualityComparer : IEqualityComparer<DecoratorField>
		{
			public bool Equals(DecoratorField x, DecoratorField y)
				=> x.Name == y.Name;

			public int GetHashCode(DecoratorField obj) => obj.Name.GetHashCode();
		}

		public CompilationUnitSyntax BuildClass(DecoratorClass decoratorClass)
		{
			return CSharpSyntaxTree.ParseText($@"public class {decoratorClass.Name} : I{decoratorClass.Name}
{{
	{DrawFields(ConcatenateFieldsOfParents(new[] { decoratorClass }))}
	public object[] Serialize()
	{{
		return DecoratorObject.Serialize(this);
	}}
}}", CSharpParseOptions.Default)
				.GetCompilationUnitRoot();
		}

		private DecoratorField[] ConcatenateFieldsOfParents(IEnumerable<DecoratorClass> decoratorClasses)
			=> decoratorClasses.SelectMany(x => x.Fields)
			.Concat(decoratorClasses.Select(x => x.Parents).SelectMany(ConcatenateFieldsOfParents))
			.Distinct(new DummyEqualityComparer())
			.ToArray();

		private string DrawFields(DecoratorField[] fields)
			=> fields.Length == 0 ? "" : fields.Select(x => $"public {x.Type} {x.Name} {{ get; set; }}").Aggregate((a, b) => $"{a}\n{b}");
	}
}