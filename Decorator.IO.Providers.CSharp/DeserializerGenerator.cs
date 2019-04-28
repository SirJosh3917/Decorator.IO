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
			=> $"{_i++}";
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

			return "// test R";
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
