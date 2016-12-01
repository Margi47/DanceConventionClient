using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DanceConventionClient
{
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage()
		{
			InitializeComponent();
			UrlEntry.Text = Application.Current.Properties["url"].ToString();
		}

		private async void Button_OnClicked(object sender, EventArgs e)
		{
			Application.Current.Properties["url"] = UrlEntry.Text;
			await Application.Current.SavePropertiesAsync();
			InfoLabel.IsVisible = true;
		}

	}
}
