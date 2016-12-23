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
		public string Name { get; set; }
		public string DivisionType { get; set; }
		public int EventId { get; set; }
	}
}
