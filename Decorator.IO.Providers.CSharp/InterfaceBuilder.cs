using Decorator.IO.Core;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.Providers.CSharp
{
	public static class InterfaceBuilder
	{
		public static IEnumerable<MemberDeclarationSyntax> BuildInterface(DecoratorClass decoratorClass)
		{
			return $@"public interface I{decoratorClass.Name} {InheritParents(decoratorClass.Parents.Select(x => x.Name))}
{{
	{DrawFields(NotInheritedFields(decoratorClass))}
}}".AsCompilationUnitSyntax()
.AsMemberDeclarationSyntaxes();
		}

		private static DecoratorField[] ConcatenateFieldsOfParents(IEnumerable<DecoratorClass> decoratorClasses)
			=> decoratorClasses.SelectMany(x => x.Fields)
			.Concat(decoratorClasses.Select(x => x.Parents).SelectMany(ConcatenateFieldsOfParents))
			.ToArray();

		private static DecoratorField[] NotInheritedFields(DecoratorClass decoratorClass)
		{
			var all = ConcatenateFieldsOfParents(decoratorClass.Parents)
				.Select(x => x.Name)
				.ToArray();

			return decoratorClass.Fields
				.Where(x => !all.Contains(x.Name))
				.ToArray();
		}

		private static string DrawFields(DecoratorField[] fields)
			=> fields.Length == 0 ? "" : fields.Select(x => $"{x.Type} {x.Name} {{ get; set; }}").Aggregate((a, b) => $"{a}\n{b}");

		private static string InheritParents(IEnumerable<string> parents)
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