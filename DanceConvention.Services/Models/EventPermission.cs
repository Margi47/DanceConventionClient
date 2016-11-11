using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanceConventionClient.Services
{
	public class EventPermission
	{
		[JsonProperty(PropertyName = "permission")]
		public string Permission { get; set; }
	}
}
