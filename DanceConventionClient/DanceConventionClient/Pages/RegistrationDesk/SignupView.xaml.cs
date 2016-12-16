﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Services;
using Xamarin.Forms;

namespace DanceConventionClient.Pages
{
	public partial class SignupView : TableView
	{

		public SignupView()
		{
			InitializeComponent();

			//InitTableInfo();
		}

		/*private void InitTableInfo()
		{
			Currency.Text = CurrentEvent.Currency;
			Status.Text = CurrentSignup.Status;

			

			AttendedButton.Text = CurrentSignup.Attended ? "ATTENDED" : "CHECK IN";
			PaymentAmount.Text = CurrentSignup.AmountOwed.ToString();
		}


		private async void Cell_OnTapped(object sender, EventArgs e)
		{
			//await Navigation.PushAsync(new UserRegistrationPage(CurrentEvent));
		}

		private async void PaymentButton_OnClicked(object sender, EventArgs e)
		{
			var amount = Decimal.Parse(PaymentAmount.Text);
			var comment = Comment.Text;

			if (amount > 0)
			{
				await _service.RecordPayment(CurrentEvent.Id, CurrentSignup.ParticipantId, amount, comment);

				Device.BeginInvokeOnMainThread((() => PaidAmount.Text = CurrentSignup.AmountPaid.ToString()));

				var signup = await _service.GetSignup(CurrentEvent.Id, CurrentSignup.ParticipantId);

				Device.BeginInvokeOnMainThread(() =>
				{
					CurrentSignup = signup;
					InitTableInfo();
				});
			}
			else
			{
				await Application.Current.MainPage.DisplayAlert("Error", "Please enter payment amount", "OK");
			}		
		}

		private async void AttendedButton_OnClicked(object sender, EventArgs e)
		{
			if (CurrentSignup.Attended)
			{
				var answer =
					await Application.Current.MainPage.DisplayAlert("Confirmation", "Do you really want to undo this check-in?", "Yes", "No");

				if (answer)
				{
					await _service.UpdateAttendanceStatus(CurrentEvent.Id, CurrentSignup.ParticipantId);
				}
			}
			else
			{
				await _service.UpdateAttendanceStatus(CurrentEvent.Id, CurrentSignup.ParticipantId);
			}

			var signup = await _service.GetSignup(CurrentEvent.Id, CurrentSignup.ParticipantId);

			Device.BeginInvokeOnMainThread(() =>
			{
				CurrentSignup = signup;
				InitTableInfo();
			});
		}*/
	}
}
