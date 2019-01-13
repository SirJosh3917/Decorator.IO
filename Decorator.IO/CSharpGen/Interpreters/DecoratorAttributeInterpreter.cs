using Decorator.IO.CSharpGen.Decorator;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Decorator.IO.CSharpGen.Interpreters
{
	public class DecoratorAttributeInterpreter : IInterpreter<string, DecoratorAttribute>
	{
		public static Dictionary<string, DecoratorAttribute> Interpretations =

			Assembly.GetExecutingAssembly()
				.GetTypes()
				.Where(type => type.BaseType == typeof(DecoratorAttribute))
				.Select(type => (type: type, aliases: type.GetCustomAttribute<AttributeNameAttribute>().Aliases))
				.SelectMany((tuple, ix) =>
				{
					var data = new (Type type, string alias)[tuple.aliases.Length];

					for (int i = 0; i < tuple.aliases.Length; i++)
					{
						data[i] = (tuple.type, tuple.aliases[i]);
					}

					return data;
				})
				.ToDictionary(kvp => kvp.alias, kvp => (DecoratorAttribute)Activator.CreateInstance(kvp.type));

		public DecoratorAttribute Interpet(string input)
			=> Interpretations.TryGetValue(input, out var result) ?
				result
				: throw new ArgumentException($"Can't interpret {nameof(input)} - no type definition for {input} exists.");
	}
}