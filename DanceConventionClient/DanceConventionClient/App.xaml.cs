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
		}

		public static void NavigateToLoginPage()
		{
			Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new NavigationPage(new LoginPage()));
		}

		public static void InitializeService(LoginResult login)
		{
			MyService = login.Service;
			Application.Current.Properties["userName"] = login.Login.Username;
			Application.Current.Properties["password"] = login.Login.Password;
		}

		public static void NavigateToMainPage()
		{
			Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new TabbedPage
			{
				Children =
				{
					new NavigationPage(new MyEventspage()),
					new ProfilePage()
				}
			});
		}


		protected override async void OnStart()
		{
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
					InitializeService(loginResult);
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
