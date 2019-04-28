using Decorator.IO.Core;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.Providers.CSharp
{
	public class InterfaceBuilder
	{
		public CompilationUnitSyntax BuildInterface(DecoratorClass decoratorClass)
		{
			return CSharpSyntaxTree.ParseText($@"public interface I{decoratorClass.Name} {InheritParents(decoratorClass.Parents.Select(x => x.Name))}
{{
	{DrawFields(NotInheritedFields(decoratorClass))}
}}", CSharpParseOptions.Default)
				.GetCompilationUnitRoot();
		}

		private DecoratorField[] ConcatenateFieldsOfParents(IEnumerable<DecoratorClass> decoratorClasses)
			=> decoratorClasses.SelectMany(x => x.Fields)
			.Concat(decoratorClasses.Select(x => x.Parents).SelectMany(ConcatenateFieldsOfParents))
			.ToArray();

		private DecoratorField[] NotInheritedFields(DecoratorClass decoratorClass)
		{
			var all = ConcatenateFieldsOfParents(decoratorClass.Parents)
				.Select(x => x.Name)
				.ToArray();

			return decoratorClass.Fields
				.Where(x => !all.Contains(x.Name))
				.ToArray();
		}

		private string DrawFields(DecoratorField[] fields)
			=> fields.Length == 0 ? "" : fields.Select(x => $"{x.Type} {x.Name} {{ get; set; }}").Aggregate((a, b) => $"{a}\n{b}");

		private string InheritParents(IEnumerable<string> parents)
		{
			if (parents.Any())
			{
				return ": " + parents.Select(x => $"I{x}")
					.Append(Config.DecoratorName)
					.Aggregate((a, b) => $"{a}, {b}");
			}

			return $": {Config.DecoratorName}";
		}
	}
}