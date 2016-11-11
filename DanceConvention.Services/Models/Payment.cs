using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanceConventionClient.Services
{
	public class Payment
	{
		[JsonProperty(PropertyName = "eventId")]
		public int EventId { get; set; }

		[JsonProperty(PropertyName = "participantId")]
		public int ParticipantId { get; set; }

		[JsonProperty(PropertyName = "paymentAmount")]
		public decimal PaymentAmount { get; set; }

		[JsonProperty(PropertyName = "comment")]
		public string Comment { get; set; }
	}
}
