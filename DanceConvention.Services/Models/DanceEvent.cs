using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanceConventionClient.Services
{
	public class DanceEvent
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string WebsiteUrl { get; set; }
		public string Description { get; set; }
		public string Location { get; set; }
		public string Currency { get; set; }
	}
}
