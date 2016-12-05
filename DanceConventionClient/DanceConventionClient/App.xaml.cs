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
using Xamarin.Forms;

namespace DanceConventionClient
{
	public partial class App : Application
	{
		public static IDCService MyService { get; set; }

		public App()
		{
			InitializeComponent();

			MainPage=new StartPage();

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

		public static async Task InitializeService(LoginResult login)
		{
			MyService = login.Service;
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
				var factory = new DCServiceFactory();
				var loginResult = await factory.Login(login);

				if (loginResult.Successful)
				{
					await InitializeService(loginResult);
					NavigateToMainPage();
					return;
				}
			}

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
