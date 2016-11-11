using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanceConventionClient.Services.Models
{
	public class LoginResult
	{
		public DCLogin Login { get; set; }
		public string ErrorMessage { get; set; }
		public DCService Service { get; set; }

		public bool Successful
		{
			get { return String.IsNullOrEmpty(ErrorMessage); } 
		}
	}
}
