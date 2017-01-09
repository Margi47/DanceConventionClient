using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using Serilog;
using Serilog.Events;
using Xamarin.Forms;

namespace DanceConventionClient.PageModels
{
	[ImplementPropertyChanged]
	public class ProfileSettingsPageModel:FreshMvvm.FreshBasePageModel
	{
		private readonly ILogger _logger;
		public string Text { get; set; }
		public static Action SendLogsToEmail;

		public ProfileSettingsPageModel()
		{
			_logger = Serilog.Log.ForContext(GetType());
		}

		public int SelectedLanguage
		{
			get
			{
				if (Application.Current.Properties.ContainsKey("language"))
				{
					if (Application.Current.Properties["language"].ToString() == "en")
					{
						return 1;
					}
					if (Application.Current.Properties["language"].ToString() == "ru")
					{
						return 2;
					}
				}
				return 0;
			}
			set
			{
				GetLanguage(value);
				_logger.Information("Language changed to {Language}", Application.Current.Properties["language"]);
				CoreMethods.DisplayAlert(AppResources.SettingsPageAlertTitle,
					AppResources.SettingsPageAlertBody, AppResources.SettingsPageAlertYes);
			}
		}

		private static async void GetLanguage(int value)
		{
			if (value == 1)
			{
				Application.Current.Properties["language"] = "en";
			}
			else if (value == 2)
			{
				Application.Current.Properties["language"] = "ru";
			}
			else
			{
				Application.Current.Properties.Remove("language");
			}

			await Application.Current.SavePropertiesAsync();
		}

		public bool SwitchToggled
		{
			get
			{
				return App.LevelSwitch.MinimumLevel == LogEventLevel.Verbose;
			}
			set
			{
				SetLogLevel(value);
				if (Application.Current.Properties["logLevel"].ToString() == "verbose")
				{
					Text = AppResources.SettingsPageInfoVerbose;
				}
				else
				{
					Text = AppResources.SettingsPageInfoInformation;
				}

				_logger.Information(Text);
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
	}
}
