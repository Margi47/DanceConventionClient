using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Services;
using Xamarin.Forms;

namespace DanceConventionClient
{
	public partial class SignupView : TableView
	{
		public Signup CurrentSignup { get; set; }
		public DanceEvent CurrentEvent { get; set; }
		private IDCService _service;

		public SignupView(DanceEvent danceEvent, Signup signup)
		{
			InitializeComponent();
			CurrentSignup = signup;
			CurrentEvent = danceEvent;
			_service = App.MyService;
			InitTableInfo();
		}

		private void InitTableInfo()
		{
			Bib.Text = CurrentSignup.BibNumber.ToString();
			Name.Text = CurrentSignup.ParticipantName;
			PaidAmount.Text = CurrentSignup.AmountPaid.ToString();
			OwnedAmount.Text = CurrentSignup.AmountOwed.ToString();
			Currency.Text = CurrentEvent.Currency;
			Status.Text = CurrentSignup.Status;
			AttendedButton.Text = CurrentSignup.Attended.ToString();
		}


		private async void Cell_OnTapped(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new UserRegistrationPage(CurrentEvent));
		}

		private async void PaymentButton_OnClicked(object sender, EventArgs e)
		{
			var amount = Decimal.Parse(PaymentAmount.Text);
			var comment = Comment.Text;
			if (amount > 0)
			{
				await _service.RecordPayment(CurrentEvent.Id, CurrentSignup.ParticipantId, amount, comment);
				PaidAmount.Text = CurrentSignup.AmountPaid.ToString();
			}
			else
			{
				await App.Current.MainPage.DisplayAlert("Error", "Please enter payment amount", "OK");
			}
			
		}

		private void AttendedButton_OnClicked(object sender, EventArgs e)
		{
			_service.UpdateAttendanceStatus(CurrentEvent.Id, CurrentSignup.ParticipantId);
		}
	}
}
