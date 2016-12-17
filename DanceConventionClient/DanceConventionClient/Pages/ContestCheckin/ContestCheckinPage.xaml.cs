using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Services;
using DanceConventionClient.Views.ContestCheckin;
using Xamarin.Forms;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace DanceConventionClient.Pages
{
	public partial class ContestCheckinPage : ContentPage
	{


		public ContestCheckinPage(DanceEvent currentEvent)
		{
			InitializeComponent();
			ProfileItem.SetProfileButton(this);

		}

		/*private async void Camera_OnClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new CameraPage() {FirstCall = true});
		}


		private async void ContestsList_OnItemTapped(object sender, ItemTappedEventArgs e)
		{
			await Navigation.PushAsync(new CurrentContestCompetitorsPage(e.Item as Contest));
		}*/
	}
}
