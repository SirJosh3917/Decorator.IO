using Decorator.IO.Core;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.Providers.CSharp
{
	public static class InterfaceBuilder
	{
		public static IEnumerable<MemberDeclarationSyntax> BuildInterface(DecoratorClass decoratorClass)
			=> $@"public interface I{decoratorClass.Name} {InheritParents(decoratorClass.Parents.Select(x => x.Name))}
{{
	{(NotInheritedFields(decoratorClass)).ToPropertyStrings(false)}
}}"
			.AsCompilationUnitSyntax()
			.AsMemberDeclarationSyntaxes();

		private static DecoratorField[] NotInheritedFields(DecoratorClass decoratorClass)
		{
			var all = decoratorClass.Parents.ConcatenateFieldsOfParents()
				.Select(x => x.Name)
				.ToArray();

			return decoratorClass.Fields
				.Where(x => !all.Contains(x.Name))
				.ToArray();
		}

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