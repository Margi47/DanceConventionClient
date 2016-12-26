using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Messaging;
using PropertyChanged;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Xamarin.Forms;

namespace DanceConventionClient.PageModels
{
	[ImplementPropertyChanged]
	public class SettingsPageModel:FreshMvvm.FreshBasePageModel
	{
		public string Url { get; set; }
		private readonly ILogger _logger;
		public string Text { get; set; }
		public static Action SendLogsToEmail;

		public bool SwitchToggled
		{
			get
			{
				return App.LevelSwitch.MinimumLevel == LogEventLevel.Verbose;
			}
			set
			{
				SetLogLevel(value);
				Text = "Log level changed to " + Application.Current.Properties["logLevel"];
			} 
		}

		private static async void SetLogLevel(bool verboseLogLevel)
		{
			if (verboseLogLevel)
			{
				App.LevelSwitch.MinimumLevel = LogEventLevel.Verbose;
				Application.Current.Properties["logLevel"] = "verbose";
			}
			else
			{
				App.LevelSwitch.MinimumLevel = LogEventLevel.Information;
				Application.Current.Properties["logLevel"] = "information";
			}
			await Application.Current.SavePropertiesAsync();
		}

		public SettingsPageModel()
		{
			_logger = Serilog.Log.ForContext(GetType());
			Url = Application.Current.Properties["url"].ToString();
		}

		public Command SendLogCommand
		{
			get
			{
				return new Command(() =>
				{
					SendLogsToEmail?.Invoke();
				});
			}
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
					Text = "URL changed to " + Url;
				});
			}
		}
	}
}
