using Decorator.IO.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.Providers.CSharp.Templates
{
	public class PropertyDefinitionArgs
	{
		public IType Type { get; set; }
		public string Identifier { get; set; }
	}

	public class PropertyDefinitionTemplate : ICSharpCodeTemplate<PropertyDefinitionArgs>
	{
		public IEnumerable<string> Generate(PropertyDefinitionArgs input)
		{
			yield return $"public {input.Type.Identifier} {input.Identifier} {{ get; set; }}";
		}
	}
}
