using System;
using Newtonsoft.Json;

namespace MemeticaMeDeLaCerda
{
	public class User
	{
		public string Id { get; set; }

		[JsonProperty(PropertyName = "username")]
		public string Username { get; set; }

		[JsonProperty(PropertyName = "deviceid")]
		public string DeviceID { get; set; }

		[JsonProperty(PropertyName = "phonenumber")]
		public string PhoneNumber { get; set; }
	}

	public class UserWrapper : Java.Lang.Object
	{
		public UserWrapper (User user)
		{
			User = user;
		}

		public User User { get; private set; }
	}
}

