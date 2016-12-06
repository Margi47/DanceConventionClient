using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Services.Models;
using Serilog;
using Xamarin.Forms;

namespace DanceConventionClient.Services
{
	public class DCServiceFactory
	{
		private readonly ILogger _logger;
		public DCServiceFactory()
		{
			_logger = Log.ForContext(GetType());
		}
		public async Task<LoginResult> Login(DCLogin login)
		{
			HttpClient httpClient = new HttpClient(new LoggingHandler(new HttpClientHandler()));

			httpClient.BaseAddress = new Uri(Application.Current.Properties["url"].ToString());

			var response = await httpClient.PostAsJsonAsync("/eventdirector/rest/mobile/auth", login);

			if (response.StatusCode == HttpStatusCode.OK)
			{
				_logger.Information("Login succeded:{@Login}", login);
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
				_logger.Information("Login invalid:{@Login}", login);
				var result = new LoginResult()
				{
					ErrorMessage = "Invalid Username or Password"
				};
				return result;
			}
			else
			{
				_logger.Information("Login failed:{@Login}", login);
				var result = new LoginResult()
				{
					ErrorMessage = "Login failed"
				};
				return result;
			}
		}
	}
}


