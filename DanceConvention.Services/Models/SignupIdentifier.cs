using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DanceConventionClient.Services.Models
{
	public class SignupIdentifier
	{
		[JsonIgnore]
		public int EventId { get; set; }
		[JsonIgnore]
		public int ParticipantId { get; set; }
	}
}
