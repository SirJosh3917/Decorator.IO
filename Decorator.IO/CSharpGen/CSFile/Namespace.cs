using System.Collections.Generic;

namespace Decorator.IO.CSharpGen.Writer
{
	public class Namespace : Child<CSFile>
	{
		public List<Class> Classes { get; set; } = new List<Class>();

		public static NamespaceOptions Options(CSFileOptions parent) => new NamespaceOptions(parent);
	}

	public class NamespaceOptions : ChildOptions<Namespace, CSFileOptions, CSFile>
	{
		public NamespaceOptions(CSFileOptions parent) : base(parent)
		{
			Building = new Namespace
			{
				Parent = parent.Building,
			};
		}

		public List<ClassOptions> Leased { get; set; } = new List<ClassOptions>();

		public override Namespace Materialize()
		{
			foreach (var due in Leased)
			{
				var materialized = due.Materialize();
				Building.Classes.Add(materialized);
			}

			return Building;
		}
	}

	public static class NamespaceExtensions
	{
		public static NamespaceOptions SetName(this NamespaceOptions namespaceOptions, string name) => namespaceOptions.SetName<NamespaceOptions, Namespace>(name);

		public static ClassOptions WithClass(this NamespaceOptions namespaceOptions)
		{
			var co = new ClassOptions(namespaceOptions);
			namespaceOptions.Leased.Add(co);
			return co;
		}
	}
}