namespace Decorator.IO.Providers.CSharp
{
	public static class Config
	{
		public const string InterfaceDecoratorObject = "IDecoratorObject";
		public const string DecoratorFactory = "DecoratorFactory";
		public const string SerializeName = "Serialize";
		public const string DeserializeName = "Deserialize";
		public const string ArrayName = "array";
		public const string ObjectName = "@object";
		public const string MessageDeserializationEventHandler = "MessageDeserializationEventHandler";
		public const string MessageDeserializationServer = "MessageDeserializationServer";
		public const string MessageBaseName = "type";
		public const string ServerName = "server";

		public static string InterfaceName(string name)
			=> $"I{name}";

		public static string SerializeAsName(string name)
			=> $"{SerializeName}As{name}";

		public static string DeserializeAsName(string name)
			=> $"{DeserializeName}As{name}";

		public static string TryDeserializeAsName(string name)
			=> $"Try{DeserializeAsName(name)}";
	}
}