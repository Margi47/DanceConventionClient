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
		public DanceEvent CurrentEvent { get; set; }
		public Signup Participant { get; set; }
	}
}
