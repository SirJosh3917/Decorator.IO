using Decorator.IO.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.Providers.CSharp
{
	public class NameGenerator
	{
		private int _i;

		public string Name()
			=> $"gen{_i++}";
	}

	public class DeserializerGenerator
	{
		private readonly DecoratorFile _context;
		private readonly DecoratorClass _classContext;
		private readonly NameGenerator _nameGenerator;

		public DeserializerGenerator(DecoratorFile context, DecoratorClass classContext, NameGenerator nameGenerator)
		{
			_context = context;
			_classContext = classContext;
			_nameGenerator = nameGenerator;
			DeserializationTable[Modifier.Required] = DeserializeRequired;
			DeserializationTable[Modifier.Optional] = DeserializeOptional;
		}

		public bool ReturnFalse { get; set; }

		public string Fail(string msg)
			=> ReturnFalse
			? $"return false; // {msg}"
			: @$"throw new System.Exception(""{msg}"");";

		public Dictionary<Modifier, Func<DecoratorField, string, string, string>> DeserializationTable = new Dictionary<Modifier, Func<DecoratorField, string, string, string>>
		{
		};

		public string DeserializeRequired(DecoratorField decoratorField, string objectName, string indexName)
		{
			var fields = _classContext.AllFieldsOf();

			var index = Array.IndexOf(fields, decoratorField);

			var cast = _nameGenerator.Name();

			// otherwise increment counter by the amount we need to advance
			int needAdvance = 0;

			if (index + 1 < fields.Length)
			{
				needAdvance = fields[index + 1].Index - fields[index].Index;
			}

			return $@"if (!({Config.ArrayName}[{indexName}] is {decoratorField.Type} {cast}))
{{
	{Fail("Type of element in object array doesn't match.")}
}}

{objectName}.{decoratorField.Name} = {cast};
{indexName} += {needAdvance};";
		}

		public string DeserializeOptional(DecoratorField decoratorField, string objectName, string indexName)
		{
			var fields = _classContext.AllFieldsOf();

			return "// test O";
		}

		public string GenerateCode(DecoratorField field, string objectName, string indexName)
		{
			return DeserializationTable[field.Modifier](field, objectName, indexName);
		}
	}
}
