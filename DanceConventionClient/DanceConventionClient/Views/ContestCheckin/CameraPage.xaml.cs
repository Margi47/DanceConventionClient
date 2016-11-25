using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Services;
using Xamarin.Forms;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace DanceConventionClient.Views.ContestCheckin
{
	public partial class CameraPage : ContentPage
	{
		private readonly IDCService _service;
		public bool FirstCall { get; set; }

		public CameraPage()
		{
			InitializeComponent();
			_service = App.MyService;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			if (FirstCall)
			{
				FirstCall = false;
				await ScanCode();
			}
		}

		public async Task ScanCode()
		{
			var scanPage = new ZXingScannerPage();
			
			scanPage.OnScanResult += (result) =>
			{
				Device.BeginInvokeOnMainThread(async () =>
				{
					scanPage.IsScanning = false;
					await Navigation.PopAsync();

					await MakeCheckin(result);
				});
			};

			await Navigation.PushAsync(scanPage);
		}
		

		private async Task MakeCheckin(Result result)
		{
			var elements = result.Text.Split('[', ':', ']');
			int eventId;
			int userId;
			if ((int.TryParse(elements[1], out eventId)) && (int.TryParse(elements[2], out userId)))
			{
				var signup = await _service.GetSignup(eventId, userId);
				await _service.AllContestCheckin(eventId, userId, signup.BibNumber, true);

				var answer = await DisplayAlert("Successful", signup.ParticipantName + " was checked-in.", "Next", "Return");

				if (!answer)
				{
					await Navigation.PopAsync();
				}
				else
				{
					await ScanCode();
				}
			}
		}

	}
}
