using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace DanceConventionClient.Views.RegistrationDesk
{
	public partial class BarcodePage : ContentPage
	{
		private ZXingBarcodeImageView _barcode;

		public BarcodePage(int eventId, int userId)
		{
			InitializeComponent();
			InitializeCode(eventId,userId);
		}

		private void InitializeCode(int eventId,int userId)
		{
			_barcode = new ZXingBarcodeImageView
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};
			_barcode.BarcodeFormat = ZXing.BarcodeFormat.QR_CODE;
			_barcode.BarcodeOptions.Width = 300;
			_barcode.BarcodeOptions.Height = 300;
			_barcode.BarcodeOptions.Margin = 10;
			_barcode.BarcodeValue = $"danceconvention.net\n[{eventId}:{userId}]";

			Content = _barcode;
		}  
	}
}
