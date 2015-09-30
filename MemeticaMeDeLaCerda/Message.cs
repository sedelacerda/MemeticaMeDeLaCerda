using System;
using Newtonsoft.Json;

namespace MemeticaMeDeLaCerda
{
	public class Message
	{
		public string id { get; set; }

		[JsonProperty(PropertyName = "emisor")]
		public string Emisor { get; set; }

		[JsonProperty(PropertyName = "receptor")]
		public string Receptor { get; set; }

		[JsonProperty(PropertyName = "texto")]
		public string Texto { get; set; }
	}

	public class MessageWrapper : Java.Lang.Object
	{
		public MessageWrapper (Message item)
		{
			Message = item;
		}

		public Message Message { get; private set; }
	}
}

