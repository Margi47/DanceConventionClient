using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanceConventionClient.Services
{
	public class ContestSignup
	{
		public int ContestId { get; set; }
		public string ContestName { get; set; }
		public string PartnerName { get; set; }
		[JsonIgnore]
		public bool HasPartner
		{
			get
			{
				return !string.IsNullOrWhiteSpace(PartnerName);
			}
		}
		public int ParticipantId { get; set; }
		public int EventId { get; set; }
		public string DivisionType { get; set; }
		public string Role { get; set; }
		[JsonIgnore]
		public bool IsLeader
		{
			get 
			{
				return Role == "LEADER";
			}
		}

		[JsonIgnore]
		public bool IsFollower
		{
			get
			{
				return Role == "FOLLOWER";
			}
		}
	}
}
