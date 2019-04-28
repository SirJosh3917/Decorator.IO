namespace Decorator.IO.Providers.CSharp
{
	public static class Config
	{
		public const string InterfaceDecoratorObject = "IDecoratorObject";
		public const string DecoratorFactory = "DecoratorFactory";
		public const string InterfaceSerializeName = "Serialize";

		public static string SerializeAsName(string name)
			=> $"SerializeAs{name}";
	}
}