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
		public int EventId { get; set; }
		public int ParticipantId { get; set; }
		public decimal PaymentAmount { get; set; }
		public string Comment { get; set; }
	}
}
