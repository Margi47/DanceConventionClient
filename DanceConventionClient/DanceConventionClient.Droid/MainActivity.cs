using System;
using System.IO;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.Threading.Tasks;
using DanceConventionClient.PageModels;
using Plugin.Messaging;
using Serilog;
using Serilog.Core;

namespace DanceConventionClient.Droid
{
	[Activity(Label = "DanceConventionClient", Icon = "@drawable/ic_launcher", Theme = "@style/MainTheme", MainLauncher = false,
		 ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		private ILogger _logger;

		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;

			global::Xamarin.Forms.Forms.Init(this, bundle);

			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.ControlledBy(App.LevelSwitch)
				.WriteTo.RollingFile(
					Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "DanceConvention_log-{Date}.txt")
					, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}",
					retainedFileCountLimit: 3)
				.WriteTo.AndroidLog()
				.CreateLogger();

			_logger = Log.ForContext(GetType());
			
			LoadApplication(new App());

			ZXing.Net.Mobile.Forms.Android.Platform.Init();

			SettingsPageModel.SendLogsToEmail = SendEmail;
			ProfileSettingsPageModel.SendLogsToEmail = SendEmail;

			AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
			TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

			System.Net.ServicePointManager.DnsRefreshTimeout = 5;
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
				if (File.Exists(Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath,
					$"DanceConvention_log-{currentDate}.txt")))
				{
					var email = new EmailMessageBuilder()
						.To("vbgmargi@gmail.com")
						.Subject("DanceConvention Client - Error Report")
						.WithAttachment(Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath,
							$"DanceConvention_log-{currentDate}.txt"))
						.Build();

					emailMessenger.SendEmail(email);
				}
				else
				{
					App.DisplayLogAlert();
				}
			}
		}

		private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
		{
			var newExc = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
			_logger.Fatal(newExc, "An error occured in TaskScheduler");
		}

		private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
		{
			var newExc = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
			_logger.Fatal(newExc, "An error occured in CurrentDomain");
		}

		protected override void OnDestroy()
		{
			Log.CloseAndFlush();
			base.OnDestroy();
		}
	}
}

