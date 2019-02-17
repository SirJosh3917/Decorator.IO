using Antlr4.Runtime.Misc;
using Decorator.IO.Core;
using Decorator.IO.Core.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decorator.IO.Parser
{
	public class Tokenizer : DIOBaseVisitor<IToken>
	{
		private List<Model> _models = new List<Model>();
		private string _ns;
		public Namespace GetNamespace() => new Namespace(_ns, _models.ToArray());

		public override IToken VisitModels([NotNull] DIOParser.ModelsContext context)
		{
			var ns = context.name_space();
			var nschars = ns.children[1].GetText();
			_ns = nschars;

			base.VisitModels(context);
			return null;
		}

		public override IToken VisitModel([NotNull] DIOParser.ModelContext context)
		{
			var parents = new Parent[0];

			var inheritingFrom = context.model_identifier()?.model_inherit()?.inheriters()?.inherit();

			if (inheritingFrom != null)
			{
				parents =
					inheritingFrom
					.Select(inherit => _models.First(model => model.Identifier == inherit.IDENTIFIER().Symbol.Text))
					.Select(model => new Parent(model))
					.ToArray();
			}

			var fields =
				GetFieldsFrom((context.fields()?.field() ?? new DIOParser.FieldContext[0]))
				.ToArray();

			var txt_identifier = context.model_identifier().IDENTIFIER().Symbol.Text;

			var result = new Model(txt_identifier, parents, fields);
			_models.Add(result);
			return result;
		}

		private IEnumerable<Field> GetFieldsFrom(IEnumerable<DIOParser.FieldContext> fields)
		{
			int counter = 0;

			foreach(var field in fields)
			{
				yield return GenerateField(field, counter);

				counter++;
			}
		}

		private Field GenerateField(DIOParser.FieldContext field, int counter)
		{
			var txt_position = field.position()?.NUMERIC()?.Symbol?.Text;
			var txt_modifier = field.modifier()?.MODIFIER()?.Symbol?.Text;
			var txt_type = field.type()?.IDENTIFIER()?.Symbol?.Text;
			var txt_identifier = field.IDENTIFIER().Symbol.Text;

			var position = Convert.ToInt32(txt_position ?? counter.ToString());
			var modifier = GetModifier(txt_modifier);
			var type = GetType(txt_type);

			return new Field(position, modifier, type, txt_identifier);
		}

		private Modifier GetModifier(string input)
		{
			switch(input)
			{
				case "REQUIRED":
				case "REQ":
				case "R":
					return Modifier.Required;

				case "OPTIONAL":
				case "OPT":
				case "O":
					return Modifier.Optional;

				case "FLATTEN":
				case "FLT":
				case "F":
					return Modifier.Flatten;

				case "ARRAY":
				case "ARR":
				case "A":
					return Modifier.Array;

				case "FLATTEN_ARRAY":
				case "FLT_ARR":
				case "FA":
					return Modifier.FlattenArray;

				default: throw new Exception($"Unknown modifier {input}");
			}
		}

		private IType GetType(string input)
		{
			switch(input)
			{
				case "STRING":
				case "STR":
				case "S":
					return new StringType();

				case "INTEGER":
				case "INT":
				case "I":
					return new IntegerType();

				case "UNSIGNED_INTEGER":
				case "U_INTEGER":
				case "UINT":
				case "UI":
					return new UnsignedIntegerType();

				case "BYTE":
				case "BYT":
				case "B":
					return new ByteType();

				default: return _models.First(x => x.Identifier == input);
			}
		}
	}
}
