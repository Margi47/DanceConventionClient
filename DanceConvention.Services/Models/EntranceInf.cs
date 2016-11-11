using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanceConventionClient.Services
{
	public class EntranceInf
	{
		[JsonProperty(PropertyName = "bibNumber")]
		public int BibNumber { get; set; }

		[JsonProperty(PropertyName = "entranceId")]
		public int EntranceId { get; set; }

		[JsonProperty(PropertyName = "participantName")]
		public string ParticipantName { get; set; }
	}
}
