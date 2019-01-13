using System;
using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.CSharpGen.Interpreters
{
	public class TypeInterpreter : IInterpreter<string, string>
	{
		private readonly Message[] _messages;

		public TypeInterpreter(Message[] messages) => _messages = messages;

		public static Dictionary<string, Type> Interpretations { get; } = new Dictionary<string, Type>
		{
			["string"] = typeof(string),
			["int"] = typeof(int)
		};

		public string Interpet(string input)
		{
			// an array
			if (input.Length > 2 &&
				input.Substring(input.Length - 2, 2) == "[]")
			{
				var baseInput = input.Substring(0, input.Length - 2);

				if (Interpretations.TryGetValue(baseInput, out var result))
				{
					return result.MakeArrayType().ToString();
				}

				Message msg;

				if ((msg = _messages.FirstOrDefault(x => x.Name == baseInput)) != null)
				{
					return $"{msg.Name}";
				}
			}
			else
			{
				if (Interpretations.TryGetValue(input, out var result))
				{
					return result.ToString();
				}

				Message msg;

				if ((msg = _messages.FirstOrDefault(x => x.Name == input)) != null)
				{
					return msg.Name;
				}
			}

			throw new ArgumentException($"Can't interpret {nameof(input)} - no type definition for {input} exists.");
		}
	}
}