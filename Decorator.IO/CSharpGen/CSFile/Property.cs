using System;

namespace Decorator.IO.CSharpGen.Writer
{
	public class Property : Child<Class>
	{
		public string Type { get; set; }
		public bool IsArray { get; set; }

		public static PropertyOptions Options(ClassOptions parent) => new PropertyOptions(parent);
	}

	public class PropertyOptions : ChildOptions<Property, ClassOptions, Class>
	{
		public PropertyOptions(ClassOptions parent) : base(parent)
		{
			Building = new Property
			{
				Parent = parent.Building,
			};
		}

		public override Property Materialize() => Building;
	}

	public static class PropertyExtensions
	{
		public static PropertyOptions SetName(this PropertyOptions PropertyOptions, string name) => PropertyOptions.SetName<PropertyOptions, Property>(name);

		public static PropertyOptions SetType<T>(this PropertyOptions propertyOptions) => propertyOptions.SetType(typeof(T));

		public static PropertyOptions SetType(this PropertyOptions propertyOptions, Type type) => propertyOptions.SetType(type.ToString());

		public static PropertyOptions SetType(this PropertyOptions propertyOptions, string type)
		{
			propertyOptions.Building.Type = type;
			return propertyOptions;
		}

		public static PropertyOptions SetArray(this PropertyOptions propertyOptions, bool isArray)
		{
			propertyOptions.Building.IsArray = isArray;
			return propertyOptions;
		}
	}
}