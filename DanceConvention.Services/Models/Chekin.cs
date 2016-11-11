using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanceConventionClient.Services
{
	public class Chekin
	{
		[JsonProperty(PropertyName = "participantId")]
		public int ParticipantId { get; set; }

		[JsonProperty(PropertyName = "bibNumber")]
		public int BibNumber { get; set; }

		[JsonProperty(PropertyName = "checkinAll")]
		public bool CheckinAll { get; set; }

	}
}
