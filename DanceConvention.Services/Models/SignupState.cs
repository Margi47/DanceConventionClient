using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanceConventionClient.Services
{
	public class SignupState
	{
		[JsonProperty(PropertyName = "targetState")]
		public string State { get; set; }
	}
}
