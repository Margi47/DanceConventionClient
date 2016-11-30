using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Services;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace DanceConventionClient.Views.RegistrationDesk
{
	public partial class RegistrationCameraPage : ContentPage
	{

		public RegistrationCameraPage(ZXingScannerView zxing)
		{
			InitializeComponent();

			MainGrid.Children.Add(zxing);
			zxing.IsScanning = true;
		}
	}
}
