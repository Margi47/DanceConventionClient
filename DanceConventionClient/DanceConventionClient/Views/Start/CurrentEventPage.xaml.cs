using DanceConventionClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DanceConventionClient
{
	public partial class CurrentEventPage : ContentPage
	{
		public DanceEvent CurrentEvent { get; set; }
		private readonly IDCService _service;

		public CurrentEventPage(DanceEvent currentEvent)
		{
			InitializeComponent();
			ProfileItem.SetProfileButton(this);

			CurrentEvent = currentEvent;
			_service = App.MyService;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			InitializeButtons();
		}

		private async void InitializeButtons()
		{			
			var permissions = await _service.GetPermissions(CurrentEvent.Id);

			var staffPermissions = new string[]
				{"EVENT_OWNER", "STAFF_EVENT_ADMIN", "STAFF_REGDESK", "STAFF_FINANCE", "STAFF_SCORES", "STAFF_CONTEST_CHECKIN"};
			var signupPermissions = new string[]
				{"SIGNED_UP_FOR_EVENT", "INVOICE_OWNER", "SIGNUP_HOST", "SIGNUP_OWNER"};

			RegDesk.IsVisible = ContestCheckin.IsVisible = permissions.Any(p => staffPermissions.Contains(p.Permission));
			MyReg.IsVisible = permissions.Any(p => signupPermissions.Contains(p.Permission));
		}

		private void MyRegistrationButtonOnClicked(object sender, EventArgs eventArgs)
		{
			Navigation.PushAsync(new UserRegistrationPage(CurrentEvent));
		}

		private void CheckinButton_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new ContestCheckinPage(CurrentEvent));
		}

		private void RegistrationDesk_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new RegistrationDeskPage(CurrentEvent));
		}
	}
}
