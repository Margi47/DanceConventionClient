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
		[JsonProperty(PropertyName = "firstName")]
		public string FirstName { get; set; }

		[JsonProperty(PropertyName = "lastName")]
		public string LastName { get; set; }

		[JsonProperty(PropertyName = "participantName")]
		public string ParticipantName { get; set; }

		[JsonProperty(PropertyName = "cityAndState")]
		public string CityAndState { get; set; }

		[JsonProperty(PropertyName = "country")]
		public string Country { get; set; }

		[JsonProperty(PropertyName = "partnerName")]
		public string PartnerName { get; set; }

		[JsonIgnore]
		public bool HasPartner
		{
			get
			{
				return !string.IsNullOrWhiteSpace(PartnerName);
			}
		}

		[JsonProperty(PropertyName = "partnerCity")]
		public string PatrnerCity { get; set; }

		[JsonProperty(PropertyName = "partnerCountry")]
		public string PartnerCountry { get; set; }

		[JsonProperty(PropertyName = "registrationDate")]
		public DateTime RegistrationDate { get; set; }

		[JsonProperty(PropertyName = "participantId")]
		public int ParticipantId { get; set; }

		[JsonProperty(PropertyName = "bibNumber")]
		public int BibNumber { get; set; }

		[JsonProperty(PropertyName = "role")]
		public string Role { get; set; }

		[JsonProperty(PropertyName = "eventname")]
		public string EventName { get; set; }

		[JsonProperty(PropertyName = "contestName")]
		public string ContestName { get; set; }

		[JsonProperty(PropertyName = "teamName")]
		public string TeamName { get; set; }

		[JsonProperty(PropertyName = "eventId")]
		public int EventId { get; set; }

		[JsonProperty(PropertyName = "checkedIn")]
		public bool CheckedIn { get; set; }

		[JsonProperty(PropertyName = "contestId")]
		public int ContestId { get; set; }

		[JsonProperty(PropertyName = "signupStatus")]
		public string SignupStatus { get; set; }
	}
}
