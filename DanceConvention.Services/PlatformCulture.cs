﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanceConventionClient.Services
{
	public class PlatformCulture
	{
		public PlatformCulture(string platformCultureString)
		{
			if (String.IsNullOrEmpty(platformCultureString))
				throw new ArgumentException("Expected culture identifier", nameof(platformCultureString));

			PlatformString = platformCultureString.Replace("_", "-");
			var dashIndex = PlatformString.IndexOf("-", StringComparison.Ordinal);
			if (dashIndex > 0)
			{
				var parts = PlatformString.Split('-');
				LanguageCode = parts[0];
				LocaleCode = parts[1];
			}
			else
			{
				LanguageCode = PlatformString;
				LocaleCode = "";
			}
		}

		public string PlatformString { get; private set; }
		public string LanguageCode { get; private set; }
		public string LocaleCode { get; private set; }

		public override string ToString()
		{
			return PlatformString;
		}
	}
}
