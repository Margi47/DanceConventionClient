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
		private ZXingScannerView _zxing;

		public CameraPage()
		{
			InitializeComponent();
			_service = App.MyService;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			InitScanner();			
		}

		public void InitScanner()
		{
			_zxing = new ZXingScannerView
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			_zxing.OnScanResult += (result) =>
				Device.BeginInvokeOnMainThread(async () => {

					_zxing.IsAnalyzing = false;

					MainGrid.Children.Clear();

					await MakeCheckin(result);
				});
			
			MainGrid.Children.Add(_zxing);
			_zxing.IsScanning = true;
		}

		private async Task MakeCheckin(Result result)
		{
			var elements = result.Text.Split('[', ':', ']');
			int eventId;
			int userId;
			if ((int.TryParse(elements[1], out eventId)) && (int.TryParse(elements[2], out userId)))
			{
				var signup = await _service.GetSignup(eventId, userId);
				if (signup.BibNumber == null)
				{
					await DisplayAlert("Error", "Not signed up for currently active competitions", "OK");
					return;
				}
				await _service.AllContestCheckin(eventId, userId, signup.BibNumber.Value, true);

				var answer = await DisplayAlert("Successful", signup.ParticipantName + " was checked-in.", "Next", "Return");

				if (!answer)
				{
					await Navigation.PopAsync();
				}
				else
				{
					InitScanner();
				}
			}
		}
	}	
}
