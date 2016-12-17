using DanceConventionClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DanceConventionClient.Pages
{
	public partial class ProfilePage : ContentPage
	{
		public ProfilePage()
		{
			InitializeComponent();
		}

		/*private void logoutButton_Clicked(object sender, EventArgs e)
		{
			Application.Current.Properties.Remove("userName");
			Application.Current.Properties.Remove("password");

			App.NavigateToLoginPage();
		}*/
	}
}
