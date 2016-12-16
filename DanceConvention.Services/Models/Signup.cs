using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DanceConventionClient.Services
{
	public class Signup
	{
		public int ParticipantId { get; set; }
		public int? BibNumber { get; set; }

		[JsonIgnore]
		public string BibNumberString
		{
			get
			{
				if (BibNumber != null)
				{
					return "[" + BibNumber + "]";
				}

				return string.Empty;
			}
		}

		public string ParticipantName { get; set; }
		public decimal AmountPaid { get; set; }
		public decimal AmountOwed { get; set; }
		public decimal AmountInvoiced { get; set; }
		public string SelectedPass { get; set; }
		public ContestSignup[] ContestSignups { get; set; }
		public string[] AvailableStateTransitions { get; set; }
		public string Status { get; set; }
		public bool Attended { get; set; }

		[JsonIgnore]
		public string AttendedString
		{
			get
			{
				return Attended ? "ATTENDED" : "CHECK IN"; ;
			}
		}
	}
}
