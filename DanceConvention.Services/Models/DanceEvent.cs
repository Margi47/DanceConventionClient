using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PropertyChanged;

namespace DanceConventionClient.Services
{
	[ImplementPropertyChanged]
	public class DanceEvent
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		[JsonIgnore]
		public string EventDates
		{
			get
			{
				return StartDate.ToString("d") + " - " + EndDate.ToString("d");
			}
		}
		public string WebsiteUrl { get; set; }
		public string Description { get; set; }
		public string Location { get; set; }
		public string Currency { get; set; }
	}
}
