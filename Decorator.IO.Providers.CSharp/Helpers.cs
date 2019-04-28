using Decorator.IO.Core;

using Humanizer;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.Providers.CSharp
{
	public static class Helpers
	{
		public static CompilationUnitSyntax AsCompilationUnitSyntax(this string str)
			=> CSharpSyntaxTree.ParseText(str, CSharpParseOptions.Default).GetCompilationUnitRoot();

		public static IEnumerable<MemberDeclarationSyntax> AsMemberDeclarationSyntaxes(this CompilationUnitSyntax compilationUnitSyntax)
			=> compilationUnitSyntax
			.ChildNodes()
			.OfType<MemberDeclarationSyntax>();

		public static string ToPropertyStrings(this IEnumerable<DecoratorField> fields, bool withPublic)
		{
			if (!fields.Any())
			{
				return "";
			}

			return fields.Select(x => $"{(withPublic ? "public " : "")} {x.Type} {x.Name} {{ get; set; }}")
				.Aggregate((a, b) => $"{a}\n{b}");
		}

		// TODO: make this more versatile by replacing Pascalize calls with
		// a Func<string, string> parameter to do the casing for us
		public static void ApplyCSharpCasing(this DecoratorFile @in)
		{
			@in.Namespace = @in.Namespace.Split('.')
				.Select(x => x.Pascalize())
				.Aggregate((a, b) => $"{a}.{b}");

			foreach (var x in @in.Classes)
			{
				x.Name = x.Name.Pascalize();
			}

			foreach (var i in @in.Classes.SelectMany(x => x.Fields))
			{
				i.Name = i.Name.Pascalize();
			}
		}

		public static IEnumerable<DecoratorField> UniqueFields(this DecoratorClass decoratorClass)
		{
			var all = decoratorClass.Parents.ConcatenateFieldsOfParents()
				.Select(x => x.Name)
				.ToArray();

			return decoratorClass.Fields
				.Where(x => !all.Contains(x.Name));
		}

		public static string InterfaceInherits(this IEnumerable<string> parents)
		{
			if (parents.Any())
			{
				return " : "
					+ parents.Select(x => $"I{x}")
					.Append(Config.DecoratorName)
					.Aggregate((a, b) => $"{a},{b}");
			}

			return " : " + Config.DecoratorName;
		}

		private static IEqualityComparer<DecoratorField> _equalityComparer = new DummyEqualityComparer();

		private class DummyEqualityComparer : IEqualityComparer<DecoratorField>
		{
			public bool Equals(DecoratorField x, DecoratorField y)
				=> x.Name == y.Name;

			public int GetHashCode(DecoratorField obj) => obj.Name.GetHashCode();
		}

		public static DecoratorField[] ConcatenateFieldsOfParents(this DecoratorClass decoratorClass)
			=> new[] { decoratorClass }.ConcatenateFieldsOfParents();

		public static DecoratorField[] ConcatenateFieldsOfParents(this IEnumerable<DecoratorClass> decoratorClasses)
			=> decoratorClasses.SelectMany(x => x.Fields)
			.Concat(decoratorClasses.Select(x => x.Parents).SelectMany(ConcatenateFieldsOfParents))
			.Distinct(_equalityComparer)
			.ToArray();
	}
}