using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DanceConventionClient.Views.Start
{
	public partial class BasePage : ContentPage
	{
		public BasePage()
		{
			InitializeComponent();

			ProfileItem.Clicked += ProfileItemOnClicked;
		}

		private void ProfileItemOnClicked(object sender, EventArgs eventArgs)
		{
			throw new NotImplementedException();
		}
	}
}
