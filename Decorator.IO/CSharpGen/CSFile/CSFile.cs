using System.Collections.Generic;

namespace Decorator.IO.CSharpGen.Writer
{
	public class CSFile : Nameable
	{
		public List<string> UsingNamespaces { get; set; } = new List<string>();
		public List<Namespace> Namespaces { get; set; } = new List<Namespace>();

		public static CSFileOptions Options() => new CSFileOptions();
	}

	public class CSFileOptions : Options<CSFile>
	{
		public List<NamespaceOptions> Leased { get; set; } = new List<NamespaceOptions>();

		public override CSFile Materialize()
		{
			foreach (var due in Leased)
			{
				var materialized = due.Materialize();
				Building.Namespaces.Add(materialized);
			}

			return Building;
		}
	}

	public static class CSFileExtensions
	{
		public static CSFileOptions SetName(this CSFileOptions cSFileOptions, string name) => cSFileOptions.SetName<CSFileOptions, CSFile>(name);

		public static CSFileOptions Using(this CSFileOptions cSFileOptions, string @namespace)
			=> cSFileOptions.FluentModify((CSFile csFile) => csFile.UsingNamespaces.Add(@namespace));

		public static NamespaceOptions WithNamespace(this CSFileOptions cSFileOptions)
		{
			var nso = new NamespaceOptions(cSFileOptions);
			cSFileOptions.Leased.Add(nso);
			return nso;
		}
	}
}