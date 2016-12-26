using System;
using System.IO;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using DanceConventionClient.PageModels;
using Plugin.Messaging;
using Serilog;
using Serilog.Core;

namespace DanceConventionClient.Droid
{
	[Activity(Label = "DanceConventionClient", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = false,
		 ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;

			global::Xamarin.Forms.Forms.Init(this, bundle);
			LoadApplication(new App());

			ZXing.Net.Mobile.Forms.Android.Platform.Init();

			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.ControlledBy(App.LevelSwitch)
				.WriteTo.RollingFile(
					Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "DanceConvention_log-{Date}.txt")
					, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}")
				.WriteTo.AndroidLog()
				.CreateLogger();

			SettingsPageModel.SendLogsToEmail = SendEmail;
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			global::ZXing.Net.Mobile.Forms.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions,
				grantResults);
		}

		public void SendEmail()
		{
			var emailMessenger = CrossMessaging.Current.EmailMessenger;
			var currentDate = DateTime.Now.ToString("yyyyMMdd");

			if (emailMessenger.CanSendEmail)
			{
				var email = new EmailMessageBuilder()
					.To("vbgmargi@gmail.com")
					.Subject("DanceConventionClientErrorReport")
					.WithAttachment(Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath,
						$"DanceConvention_log-{currentDate}.txt"))
					.Build();

				emailMessenger.SendEmail(email);
			}
		}
	}
}

