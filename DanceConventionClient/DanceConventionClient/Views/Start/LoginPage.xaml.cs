using DanceConventionClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DanceConventionClient
{
	public partial class LoginPage : ContentPage
	{
		public LoginPage()
		{
			InitializeComponent();
		}

		private async void Login(object sender, EventArgs e)
		{
			var userLogin = LoginEntry.Text;
			var userPassword = PasswordEntry.Text;

			var login = new DCLogin { Username = userLogin, Password = userPassword };
			var factory = new DCServiceFactory();
			var loginResult = await factory.Login(login);
			if (loginResult.Successful)
			{
				await App.InitializeService(loginResult);
				App.NavigateToMainPage();
			}
			else
			{
				await DisplayAlert("Error", loginResult.ErrorMessage, "OK");
			}
		}

		private async void Settings_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new SettingsPage());
		}
	}
}
