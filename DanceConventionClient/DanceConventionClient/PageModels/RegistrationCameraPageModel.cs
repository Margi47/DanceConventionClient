using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace DanceConventionClient.PageModels
{
	public class RegistrationCameraPageModel : FreshMvvm.FreshBasePageModel
	{
		public View Zxing { get; set; }

		public override void Init(object initData)
		{
			base.Init(initData);
			var zxing = initData as ZXingScannerView;
			Zxing = zxing;
			zxing.IsScanning = true;
		}
	}
}
