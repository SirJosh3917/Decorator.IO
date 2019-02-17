/*
 *
 * This class is purely becauase I don't know where I'm going with the generator.
 * With this template, it becomes much clearer what I have to do.
 *
 */

/*

NAMESPACE Example;

a:
| (0) R I integer_field
| (1) R S string_field

b:
| (2) R I another_integer_field
| (3) R S another_string_field

c [a, b]:
| (4) R I yet_another_integer_field
| (5) R I yet_another_string_field

 */

namespace Example
{
	public static class DecoratorIO
	{
		public interface IModel<T>
			where T : IModel<T>
		{
			object[] Serialize();
		}
		public static A DeserializeA(object[] data)
		{
			var instance = new A();
			// TODO: implement
			return instance;
		}
		public static object[] SerializeA(A instance)
		{
			var obj = new object[0];
			// TODO: implement
			return obj;
		}
		public static B DeserializeB(object[] data)
		{
			var instance = new B();
			// TODO: implement
			return instance;
		}
		public static object[] SerializeB(B instance)
		{
			var obj = new object[0];
			// TODO: implement
			return obj;
		}
		public static C DeserializeC(object[] data)
		{
			var instance = new C();
			// TODO: implement
			return instance;
		}
		public static object[] SerializeC(C instance)
		{
			var obj = new object[0];
			// TODO: implement
			return obj;
		}
	}
	public interface IA : DecoratorIO.IModel<IA>
	{
		int IntegerField { get; set; }
		string StringField { get; set; }
	}
	public interface IB : DecoratorIO.IModel<IB>
	{
		int AnotherIntegerField { get; set; }
		string AnotherStringField { get; set; }
	}
	public interface IC : DecoratorIO.IModel<IC>, IA, IB
	{
		int YetAnotherIntegerField { get; set; }
		string YetAnotherStringField { get; set; }
	}
	public class A : IA
	{
		public static A Deserialize(object[] data) => DecoratorIO.DeserializeA(data);
		public object[] Serialize() => DecoratorIO.SerializeA(this);

		public int IntegerField { get; set; }
		public string StringField { get; set; }
	}
	public class B : IB
	{
		public static B Deserialize(object[] data) => DecoratorIO.DeserializeB(data);
		public object[] Serialize() => DecoratorIO.SerializeB(this);

		public int AnotherIntegerField { get; set; }
		public string AnotherStringField { get; set; }
	}
	public class C : IC
	{
		public static C Deserialize(object[] data) => DecoratorIO.DeserializeC(data);
		public object[] Serialize() => DecoratorIO.SerializeC(this);

		public int YetAnotherIntegerField { get; set; }
		public string YetAnotherStringField { get; set; }
		public int IntegerField { get; set; }
		public string StringField { get; set; }
		public int AnotherIntegerField { get; set; }
		public string AnotherStringField { get; set; }
	}
}