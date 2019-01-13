using System;
using System.Collections.Generic;

namespace Decorator.IO.CSharpGen.Writer
{
	public class Method : Child<Class>
	{
		public Type ReturnType { get; set; }
		public List<MethodParameter> Parameters { get; set; } = new List<MethodParameter>();
		public List<string> Code { get; set; } = new List<string>();
		public List<string> Attributes { get; set; } = new List<string>();
		public bool IsStatic { get; set; }
		public bool IsOverride { get; set; }

		public static MethodOptions Options(ClassOptions parent) => new MethodOptions(parent);
	}

	public class MethodOptions : ChildOptions<Method, ClassOptions, Class>
	{
		public MethodOptions(ClassOptions parent) : base(parent)
		{
			Building = new Method
			{
				Parent = parent.Building,
			};
		}

		public List<MethodParameterOptions> Leased { get; set; } = new List<MethodParameterOptions>();

		public override Method Materialize()
		{
			foreach (var due in Leased)
			{
				Building.Parameters.Add(due.Materialize());
			}

			return Building;
		}
	}

	public static class MethodExtensions
	{
		public static MethodOptions SetName(this MethodOptions methodOptions, string name) => methodOptions.SetName<MethodOptions, Method>(name);

		public static MethodOptions SetReturnType<T>(this MethodOptions methodOptions) => methodOptions.SetReturnType(typeof(T));

		public static MethodOptions SetReturnType(this MethodOptions methodOptions, Type returnType)
			=> methodOptions.FluentModify((Method method) => method.ReturnType = returnType);

		public static MethodOptions SetStatic(this MethodOptions methodOptions, bool isStatic)
			=> methodOptions.FluentModify((Method method) => method.IsStatic = isStatic);

		public static MethodOptions SetOverride(this MethodOptions methodOptions, bool isOverride)
			=> methodOptions.FluentModify((Method method) => method.IsOverride = isOverride);

		public static MethodOptions AddAttribute(this MethodOptions methodOptions, string attribute)
			=> methodOptions.FluentModify((Method method) => method.Attributes.Add(attribute));

		public static MethodOptions AddCode(this MethodOptions methodOptions, string loc)
			=> methodOptions.FluentModify((Method method) => method.Code.Add(loc));

		public static MethodOptions SetCode(this MethodOptions methodOptions, params string[] loc) => methodOptions.SetCode((IEnumerable<string>)loc);

		public static MethodOptions SetCode(this MethodOptions methodOptions, IEnumerable<string> loc)
			=> methodOptions.FluentModify((Method method) => method.Code = new List<string>(loc));

		public static MethodParameterOptions WithParameter(this MethodOptions methodOptions)
		{
			var mpo = new MethodParameterOptions(methodOptions);
			methodOptions.Leased.Add(mpo);
			return mpo;
		}
	}
}