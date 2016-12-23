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
		public int ParticipantId { get; set; }
		public int BibNumber { get; set; }
		public bool CheckinAll { get; set; }
	}
}
