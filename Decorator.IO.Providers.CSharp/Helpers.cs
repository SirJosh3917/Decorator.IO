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

		public static string NewlineAggregate(this IEnumerable<string> @in)
		{
			if (@in.Any())
			{
				return @in.Aggregate((a, b) => $"{a}\n{b}");
			}

			return "";
		}

		public static string ToPropertyStrings(this IEnumerable<DecoratorField> fields, bool withPublic)
		{
			if (!fields.Any())
			{
				return "";
			}

			return fields.Select(x => $"{(withPublic ? "public " : "")} {x.Type.FullName} {x.Name} {{ get; set; }}")
				.NewlineAggregate();
		}

		// TODO: make this more versatile by replacing Pascalize calls with
		// a Func<string, string> parameter to do the casing for us
		public static void ApplyCSharpCasing(this DecoratorFile @in)
		{
			@in.Namespace = @in.Namespace.ApplyCSharpNamespaceCasing();

			foreach (var x in @in.Classes)
			{
				x.Name = x.Name.Pascalize();
			}

			foreach (var i in @in.Classes.SelectMany(x => x.Fields))
			{
				i.Name = i.Name.Pascalize();

				if (i.Type is DummyType dummyType)
				{
					dummyType.SetFullName = dummyType.SetFullName.ApplyCSharpNamespaceCasing();
				}
			}
		}

		public static string ApplyCSharpNamespaceCasing(this string str)
			=> str.Split('.')
				.Select(x => x.Pascalize())
				.Aggregate((a, b) => $"{a}.{b}");

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
				// one of the parents will inherit IDecoratorObject
				return " : "
					+ parents.Select(x => $"{Config.InterfaceName(x)}")
					.Aggregate((a, b) => $"{a},{b}");
			}

			return " : " + Config.InterfaceDecoratorObject;
		}

		private static IEqualityComparer<DecoratorField> _equalityComparer = new DummyEqualityComparer();

		private class DummyEqualityComparer : IEqualityComparer<DecoratorField>
		{
			public bool Equals(DecoratorField x, DecoratorField y)
				=> x.Name == y.Name;

			public int GetHashCode(DecoratorField obj) => obj.Name.GetHashCode();
		}

		public static DecoratorField[] AllFieldsOf(this DecoratorClass decoratorClass)
			=> decoratorClass.ConcatenateFieldsOfParents()
			.OrderBy(x => x.Index)
			.ToArray();

		public static DecoratorField[] ConcatenateFieldsOfParents(this DecoratorClass decoratorClass)
			=> new[] { decoratorClass }.ConcatenateFieldsOfParents();

		public static DecoratorField[] ConcatenateFieldsOfParents(this IEnumerable<DecoratorClass> decoratorClasses)
			=> decoratorClasses.SelectMany(x => x.Fields)
			.Concat(decoratorClasses.Select(x => x.Parents).SelectMany(ConcatenateFieldsOfParents)
				// prioritize the original enumerable classes before the others
				// prevents distinct from removing the primary fields we want
				.Where(x => !decoratorClasses.SelectMany(y => y.Fields).Contains(x)))
			.Distinct(_equalityComparer)
			.ToArray();
	}
}