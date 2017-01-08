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

		public int SelectedLanguage
		{
			get
			{
				if (Application.Current.Properties["language"] != null)
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
				Application.Current.Properties["language"] = null;
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
					Text = AppResources.SettingsPageInfoUrl + Url;
				});
			}
		}
	}
}
