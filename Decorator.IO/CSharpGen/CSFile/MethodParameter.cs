using System;

namespace Decorator.IO.CSharpGen.Writer
{
	public class MethodParameter : Child<Method>
	{
		public Type ParameterType { set => StringParameterType = value.ToString(); }
		public string StringParameterType { get; set; }
		public bool IsOut { get; set; }
		public bool IsRef { get; set; }

		public static MethodParameterOptions Options(MethodOptions parent) => new MethodParameterOptions(parent);
	}

	public class MethodParameterOptions : ChildOptions<MethodParameter, MethodOptions, Method>
	{
		public MethodParameterOptions(MethodOptions parent) : base(parent)
		{
			Building = new MethodParameter
			{
				Parent = parent.Building,
			};
		}

		public override MethodParameter Materialize() => Building;
	}

	public static class MethodParameterExtensions
	{
		public static MethodParameterOptions SetName(this MethodParameterOptions methodParameterOptions, string name) => methodParameterOptions.SetName<MethodParameterOptions, MethodParameter>(name, false);

		public static MethodParameterOptions SetParameterType<T>(this MethodParameterOptions methodParameterOptions) => methodParameterOptions.SetParameterType(typeof(T));

		public static MethodParameterOptions SetParameterType(this MethodParameterOptions methodParameterOptions, Type returnType)
			=> methodParameterOptions.FluentModify((MethodParameter MethodParameter) => MethodParameter.ParameterType = returnType);

		public static MethodParameterOptions SetParameterType(this MethodParameterOptions methodParameterOptions, string stringParameterType)
			=> methodParameterOptions.FluentModify((MethodParameter MethodParameter) => MethodParameter.StringParameterType = stringParameterType);

		public static MethodParameterOptions SetOut(this MethodParameterOptions methodParameterOptions, bool isOut)
			=> methodParameterOptions.FluentModify((MethodParameter MethodParameter) => MethodParameter.IsOut = isOut);

		public static MethodParameterOptions SetRef(this MethodParameterOptions methodParameterOptions, bool isRef)
			=> methodParameterOptions.FluentModify((MethodParameter MethodParameter) => MethodParameter.IsRef = isRef);
	}
}