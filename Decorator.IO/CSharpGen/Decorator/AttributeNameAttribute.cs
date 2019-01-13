using System;

namespace Decorator.IO.CSharpGen.Decorator
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class AttributeNameAttribute : Attribute
	{
		public AttributeNameAttribute(params string[] aliases) => Aliases = aliases;

		public string[] Aliases { get; set; }
	}
}