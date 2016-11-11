using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanceConventionClient.Services
{
	public class Contest
	{
		[JsonProperty(PropertyName = "id")]
		public int CompetitionId { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "divisionType")]
		public string DivisionType { get; set; }

		[JsonProperty(PropertyName = "eventId")]
		public int EventId { get; set; }
	}
}
