using Decorator.IO.CSharpGen.Writer;

using System;
using System.Collections.Generic;

namespace Decorator.IO.CSharpGen.Decorator
{
	public abstract class DecoratorAttribute
	{
		private string NewGuid() => Guid.NewGuid().ToString().Replace('-', '_').ToUpper();

		protected string GenVarName() => $"v__{NewGuid()}{NewGuid()}";

		public const string ArrayName = "array";
		public const string Index = "index";
		public const string Instance = "instance";
		public const string Result = "result";
		public const string Size = "size";

		public abstract IEnumerable<string> GenDeserialize(PropertyOptions appliedToOptions);

		public abstract IEnumerable<string> GenEstimateSize(PropertyOptions appliedToOptions);

		public abstract IEnumerable<string> GenSerialize(PropertyOptions appliedToOptions);
	}
}