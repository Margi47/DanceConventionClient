using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.PageModels;
using DanceConventionClient.Pages;
using DanceConventionClient.Services;
using DanceConventionClient.Services.Models;
using FreshMvvm;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Xamarin.Forms;

namespace DanceConventionClient
{
	public partial class App : Application
	{
		public static IDCService MyService { get; set; }
		public static LoggingLevelSwitch LevelSwitch = new LoggingLevelSwitch();
		private readonly ILogger _logger;

		public App()
		{
			InitializeComponent();

			_logger = Log.ForContext(GetType());

			MainPage =new StartPage();

			JsonConvert.DefaultSettings = () => new JsonSerializerSettings
			{
				DateTimeZoneHandling = DateTimeZoneHandling.Utc,
				ContractResolver = new CamelCasePropertyNamesContractResolver()
			};


			if (Properties.ContainsKey("language") && Properties["language"] != null)
			{
				var ci = new CultureInfo(Application.Current.Properties["language"].ToString());
				AppResources.Culture = ci;
				DependencyService.Get<ILocalize>().SetLocale(ci);				
			}
			else
			{
				var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
				AppResources.Culture = ci;
				DependencyService.Get<ILocalize>().SetLocale(ci);
			}
		}

		public static void NavigateToLoginPage()
		{
			var page = FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
			var container = new FreshNavigationContainer(page) { BarBackgroundColor = Color.Silver, BarTextColor = Color.Black };
			Device.BeginInvokeOnMainThread(() => App.Current.MainPage = container);
		}

		public static async Task InitializeService(LoginResult login, IDCService service)
		{
			MyService = service;
			Application.Current.Properties["userName"] = login.Login.Username;
			Application.Current.Properties["password"] = login.Login.Password;
			await App.Current.SavePropertiesAsync();
			Log.Verbose("Login information saved");
		}

		public static void NavigateToMainPage()
		{
			var page = FreshPageModelResolver.ResolvePageModel<MyEventsPageModel>();
			var container = new FreshNavigationContainer(page) {BarBackgroundColor = Color.Silver, BarTextColor = Color.Black};
			Device.BeginInvokeOnMainThread(() => App.Current.MainPage = container);
		}


		protected override async void OnStart()
		{
			if (!Properties.ContainsKey("url"))
			{
				Application.Current.Properties["url"] = "https://danceconvention.net";
			}

			if (!Properties.ContainsKey("logLevel"))
			{
				Application.Current.Properties["logLevel"] = "information";
			}

			if (Application.Current.Properties["logLevel"].ToString() == "information")
			{
				LevelSwitch.MinimumLevel = LogEventLevel.Information;
			}
			else
			{
				LevelSwitch.MinimumLevel = LogEventLevel.Verbose;
			}

			_logger.Verbose("Restoring login information");

			if (Properties.ContainsKey("userName") && Properties.ContainsKey("password"))
			{
				_logger.Verbose("Login present");

				var login = new DCLogin()
				{
					Username = Properties["userName"].ToString(),
					Password = Properties["password"].ToString()
				};
				var service = new DCServiceWrapper();

				var loginResult = await service.Login(login);
				if (loginResult == null)
				{
					_logger.Verbose("LoginResult empty, navigate to LoginPage");
					NavigateToLoginPage();
					return;
				}

				_logger.Verbose("LoginResult {@Result}", loginResult);
				if (loginResult.Successful)
				{
					await InitializeService(loginResult, service);
					_logger.Information("Navigating to events page {Url} for user {User}", Properties["url"], Properties["userName"]);
					NavigateToMainPage();
					return;
				}
			}
			else
			{
				_logger.Verbose("No login found");
			}

			_logger.Information("Navigating to login page {Url}", Properties["url"]);
			NavigateToLoginPage();
		}

		public static void DisplayLogAlert()
		{
			App.Current.MainPage.DisplayAlert(AppResources.ErrorTitle, AppResources.AppLogAlert, 
				AppResources.ErrorAnswer);

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
