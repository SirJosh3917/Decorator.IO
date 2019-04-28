using Decorator.IO.Core;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decorator.IO.Providers.CSharp
{
	public class ClassBuilder
	{
		public CompilationUnitSyntax BuildClass(DecoratorClass decoratorClass)
		{
			return CSharpSyntaxTree.ParseText($@"public class {decoratorClass.Name} : I{decoratorClass.Name}
{{
	{DrawFields(ConcatenateFieldsOfParents(new[] { decoratorClass }))}
}}", CSharpParseOptions.Default)
				.GetCompilationUnitRoot();
		}

		private DecoratorField[] ConcatenateFieldsOfParents(IEnumerable<DecoratorClass> decoratorClasses)
			=> decoratorClasses.SelectMany(x => x.Fields)
			.Concat(decoratorClasses.Select(x => x.Parents).SelectMany(ConcatenateFieldsOfParents))
			.ToArray();

		private string DrawFields(DecoratorField[] fields)
			=> fields.Length == 0 ? "" : fields.Select(x => $"public {x.Type} {x.Name} {{ get; set; }}").Aggregate((a, b) => $"{a}\n{b}");
	}
}
