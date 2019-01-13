using Decorator.IO.CSharpGen.Writer;

using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.CSharpGen.Decorator
{
	public static class CodeGeneration
	{
		public const string DeserializeName = "TryDeserialize";
		public const string EstimateSizeName = "EstimateSize";
		public const string SerializeName = "Serialize";
		public const string AggresiveInlining = "System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)";

		public static void GenerateDeserialize(this ClassOptions c, IEnumerable<(PropertyOptions, DecoratorAttribute[], Element)> methodData)
		{
			var des = GenerateDeserialize(methodData);
			var es = GenerateEstimateSize(methodData);
			var ser = GenerateSerialize(c, methodData);

			var desMethod = c.WithMethod()
				.SetName(DeserializeName)
				.SetStatic(true)
				.SetReturnType<bool>()
				.AddInstance()
				.AddObjectArray()
				.AddIndex()
				.SetCode(des);

			var esMethod = c.WithMethod()
				.SetName(EstimateSizeName)
				.SetStatic(true)
				.SetReturnType<int>()
					.WithParameter()
					.SetParameterType(c.Building.Name)
					.SetName(DecoratorAttribute.Instance)
					.Parent
				.SetCode(es);

			var serMethod = c.WithMethod()
				.SetName(SerializeName)
				.SetStatic(true)
				.SetReturnType(typeof(void))
					.WithParameter()
					.SetParameterType<object[]>()
					.SetRef(true)
					.SetName(DecoratorAttribute.ArrayName)
					.Parent

					.WithParameter()
					.SetParameterType(c.Building.Name)
					.SetName(DecoratorAttribute.Instance)
					.Parent

					.WithParameter()
					.SetParameterType<int>()
					.SetRef(true)
					.SetName(DecoratorAttribute.Index)
					.Parent
				.SetCode(ser);

			// gen a ToString that displays all the data
			c.WithMethod()
				.SetReturnType<string>()
				.SetName("ToString")
				.SetOverride(true)
				.SetCode
				(
					methodData
						.Select(x => x.Item1)
						.Select(x => $"{x.Building.Name}: {{this.{x.Building.Name}}}\\n")
						.Prepend("return $\"")
						.Append("\";")
						.Aggregate((a, b) => a + b)
				);
		}

		private static List<string> GenerateSerialize(ClassOptions c, IEnumerable<(PropertyOptions, DecoratorAttribute[], Element)> methodData)
		{
			var code = new List<string>();

			foreach (var (property, attributes, element) in methodData.OrderBy(x => x.Item3.Position))
			{
				foreach (var attribute in attributes)
				{
					code.AddRange(attribute.GenSerialize(property));
				}
			}

			code.Add($"return;");
			return code;
		}

		private static List<string> GenerateEstimateSize(IEnumerable<(PropertyOptions, DecoratorAttribute[], Element)> methodData)
		{
			var code = new List<string>
			{
				$"var {DecoratorAttribute.Size} = 0;"
			};

			foreach (var (property, attributes, element) in methodData.OrderBy(x => x.Item3.Position))
			{
				foreach (var attribute in attributes)
				{
					code.AddRange(attribute.GenEstimateSize(property));
				}
			}

			code.Add($"return {DecoratorAttribute.Size};");
			return code;
		}

		private static List<string> GenerateDeserialize(IEnumerable<(PropertyOptions, DecoratorAttribute[], Element)> methodData)
		{
			var code = new List<string>();
			int lastPos = -1;

			foreach (var (property, attributes, element) in methodData.OrderBy(x => x.Item3.Position))
			{
				foreach (var attribute in attributes)
				{
					code.AddRange(attribute.GenDeserialize(property));
				}

				code.Add($"{DecoratorAttribute.Index} += {element.Position - lastPos};");

				lastPos = element.Position;
			}

			code.Add("return true;");
			return code;
		}

		private static MethodOptions AddProxy(this ClassOptions c) => c.WithMethod().AddAttribute(AggresiveInlining);

		private static MethodOptions AddObjectArray(this MethodOptions m) => m.WithParameter().SetParameterType<object[]>().SetName(DecoratorAttribute.ArrayName).Parent;

		private static MethodOptions AddIndex(this MethodOptions m) => m.WithParameter().SetParameterType<int>().SetName(DecoratorAttribute.Index).SetRef(true).Parent;

		private static MethodOptions AddInstance(this MethodOptions m) => m.WithParameter().SetName(DecoratorAttribute.Instance).SetParameterType(m.Parent.Building.Name).Parent;

		private static MethodOptions AddOutInstance(this MethodOptions m) => m.WithParameter().SetName(DecoratorAttribute.Result).SetParameterType(m.Parent.Building.Name).SetOut(true).Parent;
	}
}