using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanceConventionClient.Services
{
	public class DCLogin
	{
		[JsonProperty(PropertyName ="username")]
		public string Username { get; set; }
		[JsonProperty(PropertyName = "password")]
		public string Password { get; set; }
	}
}
