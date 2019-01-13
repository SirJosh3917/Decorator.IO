using Decorator.IO.CSharpGen.Writer;

using System.Collections.Generic;

namespace Decorator.IO.CSharpGen.Decorator
{
	[AttributeName("flatten-array", "flat-arr", "f-a")]
	public class FlattenArrayAttribute : DecoratorAttribute
	{
		public override IEnumerable<string> GenDeserialize(PropertyOptions appliedToOptions)
		{
			var appliedTo = appliedToOptions.Building;

			var arrLength = GenVarName();
			var desArray = GenVarName();
			var desArrayIndex = GenVarName();
			var item = GenVarName();

			yield return $"if (!({CurrentElement} is int {arrLength})) return false;";
			yield return $"{Index}++;";
			yield return $"if ({arrLength} > 32767 || {arrLength} < 0) return false;"; // TODO: modify arbitrary array length
			yield return $"var {desArray} = new {appliedTo.Type}[{arrLength}];";
			yield return $"for (var {desArrayIndex} = 0; {desArrayIndex} < {desArray}.Length; {desArrayIndex}++)";
			yield return "{";
			yield return $"\tvar {item} = new {appliedTo.Type}();";
			yield return $"\tif (!{appliedTo.Type}.TryDeserialize({item}, {ArrayName}, ref {Index})) return false;";
			yield return $"\t{desArray}[{desArrayIndex}] = {item};";
			yield return "}";
			yield return $"{Instance}.{appliedTo.Name} = {desArray};";
		}

		public override IEnumerable<string> GenEstimateSize(PropertyOptions appliedToOptions)
		{
			var appliedTo = appliedToOptions.Building;

			var array = GenVarName();
			var arrayIndex = GenVarName();

			yield return $"var {array} = {Instance}.{appliedTo.Name};";
			yield return $"{Size}++;";
			yield return $"for (var {arrayIndex} = 0; {arrayIndex} < {array}.Length; {arrayIndex}++) {Size} += {appliedTo.Type}.{CodeGeneration.EstimateSizeName}({array}[{arrayIndex}]);";
		}

		public override IEnumerable<string> GenSerialize(PropertyOptions appliedToOptions)
		{
			var appliedTo = appliedToOptions.Building;

			var arrayVal = GenVarName();
			var arrayValIndex = GenVarName();

			yield return $"var {arrayVal} = {Instance}.{appliedTo.Name};";
			yield return $"{ArrayName}[{Index}++] = {arrayVal}.Length;";
			yield return $"for (var {arrayValIndex} = 0; {arrayValIndex} < {arrayVal}.Length; {arrayValIndex}++)";
			yield return "{";
			yield return $"\t{appliedTo.Type}.{CodeGeneration.SerializeName}(ref {ArrayName}, {Instance}.{appliedTo.Name}[{arrayValIndex}], ref {Index});";
			yield return "}";
		}

		private string CurrentElement => $"{ArrayName}[{Index}]";
	}
}