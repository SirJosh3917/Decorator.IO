using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Decorator.IO
{
	public class Message
	{
		[DataMember(Name = "name")] public string Name { get; set; }
		[DataMember(Name = "bases")] public string[] BaseMessages { get; set; }
		[DataMember(Name = "elements")] public Element[] Elements { get; set; }

		private string _msgName = null;
		[DataMember(Name = "msgName")] public string MessageName { get => _msgName ?? Name; set => _msgName = value; }

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
		[DataMember(Name = "type")] public string Type { get; set; }
		[DataMember(Name = "name")] public string Name { get; set; }
		[DataMember(Name = "position")] public int Position { get; set; }
		[DataMember(Name = "attributes")] public string[] Attributes { get; set; }
	}
}