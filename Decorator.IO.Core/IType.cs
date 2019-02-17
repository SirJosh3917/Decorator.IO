using System;

namespace Decorator.IO.Core
{
	public interface IType
	{
	}

	public interface INumber : IType
	{
	}

	public class StringType : IType
	{
	}

	public class IntegerType : IType, INumber
	{
	}

	public class UnsignedIntegerType : IType, INumber
	{
	}

	public class ByteType : IType, INumber
	{
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