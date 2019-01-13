using Newtonsoft.Json;

using System;
using System.Linq;

namespace Decorator.IO
{
	public class Message
	{
		[JsonProperty("name")] public string Name { get; set; }
		[JsonProperty("bases")] public string[] BaseMessages { get; set; }
		[JsonProperty("elements")] public Element[] Elements { get; set; }

		private string _msgName = null;
		[JsonProperty("msgName")] public string MessageName { get => _msgName ?? Name; set => _msgName = value; }

		public Message Clone()
			=> new Message
			{
				Name = Name,
				BaseMessages = BaseMessages,
				Elements = Elements,
				MessageName = MessageName,
			};

		public Element[] InheritBase(Message[] allMsgs)
		{
			if (BaseMessages == null || BaseMessages.Length == 0)
			{
				return Elements;
			}

			var tmpElems = Elements;

			foreach (var baseMsg in allMsgs.Where(x => x.Name == null ? false : BaseMessages.Contains(x.Name)))
			{
				tmpElems =
					tmpElems
					.Concat(baseMsg.InheritBase(allMsgs))
					.ToArray();
			}

			return tmpElems;
		}
	}

	public class Element
	{
		[JsonProperty("type")] public string Type { get; set; }
		[JsonProperty("name")] public string Name { get; set; }
		[JsonProperty("position")] public int Position { get; set; }
		[JsonProperty("attributes")] public string[] Attributes { get; set; }
	}
}