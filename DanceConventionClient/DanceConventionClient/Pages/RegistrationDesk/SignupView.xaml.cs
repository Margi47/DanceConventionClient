using System;
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

		
		}*/
	}
}
