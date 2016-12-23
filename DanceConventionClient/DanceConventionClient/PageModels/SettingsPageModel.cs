using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using Serilog;
using Xamarin.Forms;

namespace DanceConventionClient.PageModels
{
	[ImplementPropertyChanged]
	public class SettingsPageModel:FreshMvvm.FreshBasePageModel
	{
		public string Url { get; set; }
		private readonly ILogger _logger;
		public bool ShowInfo { get; set; }

		public SettingsPageModel()
		{
			_logger = Serilog.Log.ForContext(GetType());
			Url = Application.Current.Properties["url"].ToString();
			ShowInfo = false;
		}

		public Command UrlCommand
		{
			get
			{
				return new Command(async() =>
				{
					Application.Current.Properties["url"] = Url;
					_logger.Information("Changing url to {Url}", Url);
					await Application.Current.SavePropertiesAsync();
					ShowInfo = true;
				});
			}
		}
	}
}
