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

			_logger.Information("CurrentCulture set: " + ci.Name);
		}

		public CultureInfo GetCurrentCultureInfo()
		{
			var netLanguage = "en";
			var androidLocale = Java.Util.Locale.Default;
			netLanguage = AndroidToDotnetLanguage(androidLocale.ToString().Replace("_", "-"));

			// this gets called a lot - try/catch can be expensive so consider caching or something
			CultureInfo ci = null;
			try
			{
				ci = new CultureInfo(netLanguage);
			}
			catch (CultureNotFoundException e1)
			{
				// iOS locale not valid .NET culture (eg. "en-ES" : English in Spain)
				// fallback to first characters, in this case "en"
				try
				{
					var fallback = ToDotnetFallbackLanguage(new PlatformCulture(netLanguage));
					_logger.Information(netLanguage + " failed, trying " + fallback + " (" + e1.Message + ")");
					ci = new CultureInfo(fallback);
				}
				catch (CultureNotFoundException e2)
				{
					// iOS language not valid .NET culture, falling back to English
					_logger.Information(netLanguage + " couldn't be set, using 'en' (" + e2.Message + ")");
					ci = new CultureInfo("en");
				}
			}

			return ci;
		}

		string AndroidToDotnetLanguage(string androidLanguage)
		{
			_logger.Information("Android Language:" + androidLanguage);
			var netLanguage = androidLanguage;

			//certain languages need to be converted to CultureInfo equivalent
			/*switch (androidLanguage)
			{
				case "ms-BN":   // "Malaysian (Brunei)" not supported .NET culture
				case "ms-MY":   // "Malaysian (Malaysia)" not supported .NET culture
				case "ms-SG":   // "Malaysian (Singapore)" not supported .NET culture
					netLanguage = "ms"; // closest supported
					break;
				case "in-ID":  // "Indonesian (Indonesia)" has different code in  .NET 
					netLanguage = "id-ID"; // correct code for .NET
					break;
				case "gsw-CH":  // "Schwiizert��tsch (Swiss German)" not supported .NET culture
					netLanguage = "de-CH"; // closest supported
					break;
					// add more application-specific cases here (if required)
					// ONLY use cultures that have been tested and known to work
			}*/

			_logger.Information(".NET Language/Locale:" + netLanguage);
			return netLanguage;
		}

		string ToDotnetFallbackLanguage(PlatformCulture platCulture)
		{
			_logger.Information(".NET Fallback Language:" + platCulture.LanguageCode);
			var netLanguage = platCulture.LanguageCode; // use the first part of the identifier (two chars, usually);

			switch (platCulture.LanguageCode)
			{
				case "gsw":
					netLanguage = "de-CH"; // equivalent to German (Switzerland) for this app
					break;
					// add more application-specific cases here (if required)
					// ONLY use cultures that have been tested and known to work
			}

			//Console.WriteLine(".NET Fallback Language/Locale:" + netLanguage + " (application-specific)");
			return netLanguage;
		}
	}
}