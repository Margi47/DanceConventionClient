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


		/*

		private async Task ShowSignup(Result result)
		{
			var elements = result.Text.Split('[', ':', ']');
			int eventId;
			int userId;

			if ((int.TryParse(elements[1], out eventId)) && (int.TryParse(elements[2], out userId)))
			{
				var signup = await _service.GetSignup(eventId, userId);
				ContentStack.Children.Add(new SignupView(CurrentEvent, signup));
			}
		}*/
	}
}
