using Decorator.IO.CSharpGen.Writer;

using System.Collections.Generic;

namespace Decorator.IO.CSharpGen.Decorator
{
	[AttributeName("required", "req", "r")]
	public class RequiredAttribute : DecoratorAttribute
	{
		public override IEnumerable<string> GenDeserialize(PropertyOptions appliedToOptions)
		{
			var appliedTo = appliedToOptions.Building;
			var varName = GenVarName();

			yield return $"if ({ArrayName}.Length <= {Index}) return false;";
			yield return $"if (!({ArrayName}[{Index}] is {appliedTo.Type} {varName})) return false;";
			yield return $"{Instance}.{appliedTo.Name} = {varName};";
		}

		public override IEnumerable<string> GenEstimateSize(PropertyOptions appliedToOptions)
		{
			var appliedTo = appliedToOptions.Building;
			yield return $"{Size}++;";
		}

		public override IEnumerable<string> GenSerialize(PropertyOptions appliedToOptions)
		{
			var appliedTo = appliedToOptions.Building;
			yield return $"{ArrayName}[{Index}++] = {Instance}.{appliedTo.Name};";
		}
	}
}