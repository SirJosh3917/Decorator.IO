namespace Decorator.IO.Providers.CSharp
{
	public static class Config
	{
		public const string InterfaceDecoratorObject = "IDecoratorObject";
		public const string DecoratorFactory = "DecoratorFactory";
		public const string SerializeName = "Serialize";
		public const string DeserializeName = "Deserialize";

		public static string SerializeAsName(string name)
			=> $"{SerializeName}As{name}";

		public static string DeserializeAsName(string name)
			=> $"{DeserializeName}As{name}";
	}
}