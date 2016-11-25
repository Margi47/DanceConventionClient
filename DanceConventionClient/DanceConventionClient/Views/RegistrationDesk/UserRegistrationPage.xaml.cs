using DanceConventionClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Views.RegistrationDesk;
using Xamarin.Forms;

namespace DanceConventionClient
{
	public partial class UserRegistrationPage : ContentPage
	{
		private readonly IDCService _service;
		public DanceEvent CurrentEvent { get; set; }
		public Signup CurrentSignup { get; set; }

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
			CurrentSignup = await _service.GetSignup(CurrentEvent.Id, profile.Id);

			Place.Text = CurrentEvent.Location;
			name.Text = CurrentSignup.ParticipantName;
			pass.Detail = CurrentSignup.SelectedPass;
			invoicedAmount.Detail = CurrentSignup.AmountInvoiced.ToString();
			paidAmount.Detail = CurrentSignup.AmountPaid.ToString();

			contestsList.ItemsSource = CurrentSignup.ContestSignups;
		}

		private async void qrCode_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new BarcodePage(CurrentEvent.Id, CurrentSignup.ParticipantId));

		}
	}
}
