using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Services.Models;

namespace DanceConventionClient.Services
{
	public class DCServiceFactory
	{
		public async Task<LoginResult> Login(DCLogin login)
		{
			HttpClient httpClient = new HttpClient(new LoggingHandler(new HttpClientHandler()));

			httpClient.BaseAddress = new Uri("https://sandbox.danceconvention.net/");

			var response = await httpClient.PostAsJsonAsync("/eventdirector/rest/mobile/auth", login);

			if (response.StatusCode == HttpStatusCode.OK)
			{
				var service = new DCService(httpClient);
				var result = new LoginResult()
				{
					Login = login,
					Service = service
				};
				return result;
			}
			else if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				var result = new LoginResult()
				{
					ErrorMessage = "Invalid Username or Password"
				};
				return result;
			}
			else
			{
				var result = new LoginResult()
				{
					ErrorMessage = "Login failed"
				};
				return result;
			}
		}
	}
}


