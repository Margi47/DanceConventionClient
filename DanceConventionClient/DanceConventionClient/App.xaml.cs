using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Services;
using DanceConventionClient.Services.Models;
using Newtonsoft.Json;
using Serilog;
using Xamarin.Forms;

namespace DanceConventionClient
{
	public partial class App : Application
	{
		public static IDCService MyService { get; set; }
		private readonly ILogger _logger;

		public App()
		{
			InitializeComponent();

			_logger = Log.ForContext(GetType());

			MainPage =new StartPage();

			Device.BeginInvokeOnMainThread((() =>
			{
				JsonConvert.DefaultSettings = () => new JsonSerializerSettings
				{
					DateTimeZoneHandling = DateTimeZoneHandling.Utc
				};
			}));
		}

		public static void NavigateToLoginPage()
		{
			Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new NavigationPage(new LoginPage()));
		}

		public static async Task InitializeService(LoginResult login, IDCService service)
		{
			MyService = service;
			Application.Current.Properties["userName"] = login.Login.Username;
			Application.Current.Properties["password"] = login.Login.Password;
			await App.Current.SavePropertiesAsync();
		}

		public static void NavigateToMainPage()
		{
			Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new NavigationPage(new MyEventspage()));
		}


		protected override async void OnStart()
		{
			if (!Properties.ContainsKey("url"))
			{
				Application.Current.Properties["url"] = "https://danceconvention.net";
			}

			if (Properties.ContainsKey("userName") && Properties.ContainsKey("password"))
			{
				var login = new DCLogin()
				{
					Username = Properties["userName"].ToString(),
					Password = Properties["password"].ToString()
				};
				var service = new DCServiceVrapper();
				var loginResult = await service.Login(login);

				if (loginResult.Successful)
				{
					
					await InitializeService(loginResult, service);
					NavigateToMainPage();
					_logger.Information("Navigation to events page with properties:{UserName}, {Password}, {Url}", Properties["userName"], Properties["password"], Properties["url"]);
					return;
				}
			}

			NavigateToLoginPage();
			_logger.Information("Navigation to login page with properties:{UserName}, {Password}, {Url}", Properties["userName"], Properties["password"], Properties["url"]);
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
