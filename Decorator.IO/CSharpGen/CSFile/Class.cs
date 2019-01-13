using System.Collections.Generic;

namespace Decorator.IO.CSharpGen.Writer
{
	public class Class : Child<Namespace>
	{
		public List<Property> Properties { get; set; } = new List<Property>();
		public List<Method> Methods { get; set; } = new List<Method>();

		public static ClassOptions Options(NamespaceOptions parent) => new ClassOptions(parent);
	}

	public class ClassOptions : ChildOptions<Class, NamespaceOptions, Namespace>
	{
		public ClassOptions(NamespaceOptions parent) : base(parent)
		{
			Building = new Class
			{
				Parent = parent.Building,
			};
		}

		public List<MethodOptions> LeasedMethods { get; set; } = new List<MethodOptions>();
		public List<PropertyOptions> LeasedProperties { get; set; } = new List<PropertyOptions>();

		public override Class Materialize()
		{
			foreach (var due in LeasedMethods)
			{
				Building.Methods.Add(due.Materialize());
			}

			foreach (var due in LeasedProperties)
			{
				Building.Properties.Add(due.Materialize());
			}

			return Building;
		}
	}

	public static class ClassExtensions
	{
		public static ClassOptions SetName(this ClassOptions classOptions, string name) => classOptions.SetName<ClassOptions, Class>(name);

		public static PropertyOptions WithProperty(this ClassOptions classOptions)
		{
			var po = new PropertyOptions(classOptions);
			classOptions.LeasedProperties.Add(po);
			return po;
		}

		public static MethodOptions WithMethod(this ClassOptions classOptions)
		{
			var mo = new MethodOptions(classOptions);
			classOptions.LeasedMethods.Add(mo);
			return mo;
		}
	}
}