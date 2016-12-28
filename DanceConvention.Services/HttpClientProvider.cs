using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DanceConventionClient.Services
{
	public class HttpClientProvider
	{
		private readonly CookieContainer _container = new CookieContainer();
		public HttpClient Client { get; private set; }

		public HttpClientProvider()
		{
			InitClient();
		}

		public void InitClient()
		{
			var clientHandler = new HttpClientHandler();
			clientHandler.CookieContainer = _container;
			Client = new HttpClient(new LoggingHandler(clientHandler));
			Client.BaseAddress = new Uri(Application.Current.Properties["url"].ToString());
		}
	}
}
