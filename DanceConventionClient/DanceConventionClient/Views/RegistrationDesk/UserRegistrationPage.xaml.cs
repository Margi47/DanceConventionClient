using DanceConventionClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DanceConventionClient
{
	public partial class UserRegistrationPage : ContentPage
	{
		private readonly IDCService _service;
		public DanceEvent CurrentEvent { get; set; }

		public UserRegistrationPage(DanceEvent currentEvent)
		{
			InitializeComponent();
			CurrentEvent = currentEvent;
			_service = App.MyService;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			var profile = await _service.GetProfile(); 
			var signup = await _service.GetSignup(CurrentEvent.Id, profile.Id);

			place.Text = CurrentEvent.Location;
			name.Text = signup.ParticipantName;
			pass.Detail = signup.SelectedPass;
			invoicedAmount.Detail = signup.AmountInvoiced.ToString();
			paidAmount.Detail = signup.AmountPaid.ToString();

			contestsList.ItemsSource = signup.ContestSignups;
		}

		private void qrCode_Clicked(object sender, EventArgs e)
		{

		}
	}
}
