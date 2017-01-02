﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using Serilog.Events;
using Xamarin.Forms;

namespace DanceConventionClient.PageModels
{
	[ImplementPropertyChanged]
	public class ProfileSettingsPageModel:FreshMvvm.FreshBasePageModel
	{
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
