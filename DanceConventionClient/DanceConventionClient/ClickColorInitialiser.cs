using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DanceConventionClient
{
	public class ClickColorInitialiser
	{
		public static void ChangeColor(Color propertyName)
		{
			propertyName = Color.Gray;
			Device.StartTimer(TimeSpan.FromSeconds(0.25), () =>
			{
				propertyName = Color.Transparent;
				return false;
			});
		}
	}
}
