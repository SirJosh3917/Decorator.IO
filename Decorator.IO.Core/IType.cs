using System;

namespace Decorator.IO.Core
{
	public interface IType
	{
		string Identifier { get; }
		bool IsValueType { get; }
	}

	public interface INumber : IType
	{
	}

	public class VoidType : IType
	{
		public string Identifier => "void";
		public bool IsValueType => false;
	}

	public class StringType : IType
	{
		public string Identifier => "string";
		public bool IsValueType => false;
	}

	public class IntegerType : IType, INumber
	{
		public string Identifier => "int";
		public bool IsValueType => true;
	}

	public class UnsignedIntegerType : IType, INumber
	{
		public string Identifier => "uint";
		public bool IsValueType => true;
	}

	public class ByteType : IType, INumber
	{
		public string Identifier => "byte";
		public bool IsValueType => false;
	}

	public static class ITypeExtensions
	{
		public static IType SwitchIf<T>(this IType type, Action<T> ifType)
			where T : IType
		{
			if (type is T typeIsT)
			{
				ifType?.Invoke(typeIsT);
			}

			return type;
		}
	}
}