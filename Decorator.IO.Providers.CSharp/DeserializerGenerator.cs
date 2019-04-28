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
	throw new System.Exception(""todo: make try deserialize alternative and abstract the heck outta crud lol"");
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
