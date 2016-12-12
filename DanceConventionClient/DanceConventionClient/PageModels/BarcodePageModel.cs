using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Services.Models;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace DanceConventionClient.PageModels
{
	public class BarcodePageModel:FreshMvvm.FreshBasePageModel
	{
		public ZXingBarcodeImageView Barcode { get; set; }

		public BarcodePageModel()
		{
			Barcode = new ZXingBarcodeImageView
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};
		}

		public override void Init(object initData)
		{
			base.Init(initData);
			var identifier = initData as SignupIdentifier;
			InitializeCode(identifier.EventId, identifier.ParticipantId);
		}

		private void InitializeCode(int eventId, int userId)
		{			
			Barcode.BarcodeFormat = ZXing.BarcodeFormat.QR_CODE;
			Barcode.BarcodeOptions.Width = 300;
			Barcode.BarcodeOptions.Height = 300;
			Barcode.BarcodeOptions.Margin = 10;
			Barcode.BarcodeValue = $"danceconvention.net\n[{eventId}:{userId}]";
		}
	}
}
