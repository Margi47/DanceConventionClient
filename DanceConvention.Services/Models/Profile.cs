using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanceConventionClient.Services
{
	public class Profile
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		[JsonIgnore]
		public string FullName => FirstName + " " + LastName;

		public string Email { get; set; }
		public string Role { get; set; }
		public string PreferredLang { get; set; }
	}
}
