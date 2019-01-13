using Decorator.IO.CSharpGen.Decorator;
using Decorator.IO.CSharpGen.Interpreters;
using Decorator.IO.CSharpGen.Writer;

using Humanizer;

using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.CSharpGen
{
	public class CSharpGenerator : ICodeGenerator
	{
		public CSharpGenerator(string @namespace)
		{
			_ns = _file.WithNamespace()
				.SetName($"{@namespace}.Models");
		}

		private readonly CSFileOptions _file = new CSFileOptions();
		private readonly NamespaceOptions _ns;
		private readonly DecoratorAttributeInterpreter _dai = new DecoratorAttributeInterpreter();
		private TypeInterpreter _ti;

		private List<Message> _msgs = new List<Message>();

		public string Generate() => _file.Materialize().Generate();

		public void WorkOn(IEnumerable<Message> messages)
		{
			var msgArray = messages.ToArray();

			_ti = new TypeInterpreter(msgArray);

			foreach (var message in msgArray)
			{
				AddMessage(message);
			}
		}

		private void AddMessage(Message message)
		{
			_msgs.Add(message);

			var c = _ns.WithClass()
				.SetName(message.Name);

			var methodData = new List<(PropertyOptions, DecoratorAttribute[], Element)>();

			foreach (var element in message.Elements)
			{
				var property = c.WithProperty()
					.SetName(element.Name)
					.SetType(_ti.Interpet(element.Type).Pascalize())
					.SetArray(element.Type.EndsWith("[]"));

				var attributes =
					element.Attributes
					.Select(x => _dai.Interpet(x))
					.ToArray();

				methodData.Add((property, attributes, element));
			}

			c.GenerateDeserialize(methodData);
		}
	}
}