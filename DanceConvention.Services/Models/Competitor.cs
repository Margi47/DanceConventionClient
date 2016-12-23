using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanceConventionClient.Services
{
	public class Competitor
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string ParticipantName { get; set; }
		public string CityAndState { get; set; }
		public string Country { get; set; }
		public string PartnerName { get; set; }

		[JsonIgnore]
		public bool HasPartner => !string.IsNullOrWhiteSpace(PartnerName);

		public string PatrnerCity { get; set; }
		public string PartnerCountry { get; set; }
		public DateTime RegistrationDate { get; set; }
		public int ParticipantId { get; set; }
		public int BibNumber { get; set; }

		[JsonIgnore]
		public string BibNumberString => "[" + BibNumber + "]";

		public string Role { get; set; }
		public string EventName { get; set; }
		public string ContestName { get; set; }
		public string TeamName { get; set; }
		public int EventId { get; set; }
		public bool CheckedIn { get; set; }
		public int ContestId { get; set; }
		public string SignupStatus { get; set; }
	}
}
