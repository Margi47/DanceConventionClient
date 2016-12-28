using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Services;
using PropertyChanged;
using Xamarin.Forms;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace DanceConventionClient.PageModels
{
	[ImplementPropertyChanged]
	public class CameraPageModel : FreshMvvm.FreshBasePageModel
	{
		private readonly IDCService _service;
		public View ScannerView { get; set; }
		public bool ShowScanner { get; set; }
		public bool IsLoading { get; set; }

		public CameraPageModel()
		{
			_service = App.MyService;
		}

		public override void Init(object initData)
		{
			base.Init(initData);
			InitScanner();
		}

		public void InitScanner()
		{
			var zxing = new ZXingScannerView
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			zxing.OnScanResult += (result) =>
				Device.BeginInvokeOnMainThread(async () =>
				{

					zxing.IsAnalyzing = false;
					ScannerView = null;
					await MakeCheckin(result);
				});

			ScannerView = zxing;
			ShowScanner = true;
			zxing.IsScanning = true;
		}

		private async Task MakeCheckin(Result result)
		{
			var elements = result.Text.Split('[', ':', ']');
			int eventId;
			int userId;
			if ((int.TryParse(elements[1], out eventId)) && (int.TryParse(elements[2], out userId)))
			{
				var signup = await _service.GetSignup(eventId, userId);
				if (signup == null)
				{
					return;
				}

				if (signup.BibNumber == null)
				{
					await CoreMethods.DisplayAlert("Error", "Not signed up for currently active competitions", "OK");
					return;
				}

				IsLoading = true;
				await _service.AllContestCheckin(eventId, userId, signup.BibNumber.Value, true);
				IsLoading = false;
				var answer =
					await CoreMethods.DisplayAlert("Successful", signup.ParticipantName + " was checked-in.", "Next", "Return");

				if (!answer)
				{
					await CoreMethods.PopPageModel();
				}
				else
				{
					InitScanner();
				}
			}
		}
	}
}
