using Humanizer;

using System;

namespace Decorator.IO.CSharpGen.Writer
{
	public abstract class ChildOptions<T, TChildOptions, TChild> : Options<T>
		where T : Nameable, new()
		where TChildOptions : Options<TChild>
		where TChild : Nameable, new()
	{
		protected ChildOptions(TChildOptions childOptions) => Parent = childOptions;

		public TChildOptions Parent { get; }
	}

	public abstract class Options<T> where T : Nameable, new()
	{
		public T Building { get; set; } = new T();

		public abstract T Materialize();
	}

	public static class OptionsExtensions
	{
		public static T FluentModify<T, T2>(this T options, Action<T2> modify)
			where T : Options<T2>
			where T2 : Nameable, new()
		{
			modify?.Invoke(options.Building);
			return options;
		}

		public static T SetName<T, T2>(this T options, string name, bool pascal = true)
			where T : Options<T2>
			where T2 : Nameable, new()
			=> options.FluentModify((T2 item) => item.Name = pascal ? name.Pascalize() : name.Camelize());
	}
}