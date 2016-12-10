using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Xamarin.Forms;

namespace DanceConventionClient
{
	public partial class SettingsPage : ContentPage
	{
		private readonly ILogger _logger;

		public SettingsPage()
		{
			InitializeComponent();
			UrlEntry.Text = Application.Current.Properties["url"].ToString();
			_logger = Serilog.Log.ForContext(GetType());
		}

		private async void Button_OnClicked(object sender, EventArgs e)
		{
			Application.Current.Properties["url"] = UrlEntry.Text;
			_logger.Information("Changing url to {Url}", UrlEntry.Text);
			await Application.Current.SavePropertiesAsync();
			InfoLabel.IsVisible = true;
		}

	}
}
