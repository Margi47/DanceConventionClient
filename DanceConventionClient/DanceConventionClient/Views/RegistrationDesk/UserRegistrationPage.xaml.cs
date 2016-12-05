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
			ProfileItem.SetProfileButton(this);

			CurrentEvent = currentEvent;
			_service = App.MyService;
			Title = CurrentEvent.Name;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			var profile = await _service.GetProfile(); 
			CurrentSignup = await _service.GetSignup(CurrentEvent.Id, profile.Id);


			Device.BeginInvokeOnMainThread(() =>
			{
				AllContestsList.ItemsSource = CurrentSignup.ContestSignups;
				Place.Text = CurrentEvent.Location;
				Date.Text = CurrentEvent.EventDates;
				Name.Text = CurrentSignup.ParticipantName;
				Pass.Text = CurrentSignup.SelectedPass;
				InvoicedAmount.Text = CurrentSignup.AmountInvoiced.ToString();
				PaidAmount.Text = CurrentSignup.AmountPaid.ToString();

				if(CurrentSignup.AmountPaid < CurrentSignup.AmountInvoiced)
				{
					PaidAmount.TextColor = Color.Red;
					PaidLabel.TextColor = Color.Red;
				}
				else
				{
					PaidAmount.TextColor = Color.Blue;
					PaidLabel.TextColor = Color.Blue;
				}
			});
		}

		private async void qrCode_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new BarcodePage(CurrentEvent.Id, CurrentSignup.ParticipantId));

		}
	}
}
