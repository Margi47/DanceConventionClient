using DanceConventionClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Views.RegistrationDesk;
using Xamarin.Forms;

namespace DanceConventionClient.Pages
{
	public partial class UserRegistrationPage : ContentPage
	{

		public UserRegistrationPage(DanceEvent currentEvent)
		{
			InitializeComponent();
			ProfileItem.SetProfileButton(this);
		}

	}
}
