using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.PageModels;
using DanceConventionClient.Pages;
using DanceConventionClient.Services;
using DanceConventionClient.Services.Models;
using FreshMvvm;
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

			JsonConvert.DefaultSettings = () => new JsonSerializerSettings
			{
				DateTimeZoneHandling = DateTimeZoneHandling.Utc
			};
		}

		public static void NavigateToLoginPage()
		{
			var page = FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
			var container = new FreshNavigationContainer(page);
			Device.BeginInvokeOnMainThread(() => App.Current.MainPage = container);
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
			var page = FreshPageModelResolver.ResolvePageModel<MyEventsPageModel>();
			var container = new FreshNavigationContainer(page);
			Device.BeginInvokeOnMainThread(() => App.Current.MainPage = container);
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
				var service = new DCServiceWrapper();
				var loginResult = await service.Login(login);

				if (loginResult.Successful)
				{
					await InitializeService(loginResult, service);
					_logger.Information("Navigating to events page {Url} for user {User}", Properties["url"], Properties["userName"]);					
					NavigateToMainPage();
					return;
				}
			}

			_logger.Information("Navigating to login page {Url}", Properties["url"]);
			NavigateToLoginPage();
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
