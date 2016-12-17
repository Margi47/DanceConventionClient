using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Services;
using DanceConventionClient.Views.RegistrationDesk;
using Xamarin.Forms;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace DanceConventionClient.Pages
{
	public partial class RegistrationDeskPage : ContentPage
	{
		public RegistrationDeskPage()
		{
			InitializeComponent();
			ProfileItem.SetProfileButton(this);
		}
	}
}
