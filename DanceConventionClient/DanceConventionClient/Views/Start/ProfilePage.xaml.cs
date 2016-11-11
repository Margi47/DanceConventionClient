using DanceConventionClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DanceConventionClient
{
	public partial class ProfilePage : ContentPage
	{
		public IDCService Service= App.MyService;
		public ProfilePage()
		{
			InitializeComponent();
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await InitializeData();
		}

		private async Task InitializeData()
		{
			var currentProfile = await Service.GetProfile();

			profileName.Text = currentProfile.FirstName + " " + currentProfile.LastName;
			email.Text = currentProfile.Email;
			accessPermission.Text = currentProfile.Role;
		}

		private void logoutButton_Clicked(object sender, EventArgs e)
		{
			Application.Current.Properties.Remove("userName");
			Application.Current.Properties.Remove("password");

			App.NavigateToLoginPage();


		}
	}
}
