using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Serilog;
using Xamarin.Forms;

[assembly: Dependency(typeof(DanceConventionClient.Droid.Localize))]
namespace DanceConventionClient.Droid
{
	public class Localize:ILocalize
	{
		private readonly ILogger _logger;

		public Localize()
		{
			_logger = Log.ForContext(GetType());
		}

		public void SetLocale(CultureInfo ci)
		{
			Thread.CurrentThread.CurrentCulture = ci;
			Thread.CurrentThread.CurrentUICulture = ci;

			_logger.Information("CurrentCulture set to {Culture}", ci.Name);
		}

		public CultureInfo GetCurrentCultureInfo()
		{
			var netLanguage = "en";
			var androidLocale = Java.Util.Locale.Default;
			netLanguage = androidLocale.ToString().Replace("_", "-");

			CultureInfo ci = null;
			try
			{
				ci = new CultureInfo(netLanguage);
			}
			catch (CultureNotFoundException e1)
			{
				ci = new CultureInfo("en");
			}

			return ci;
		}
	}
}