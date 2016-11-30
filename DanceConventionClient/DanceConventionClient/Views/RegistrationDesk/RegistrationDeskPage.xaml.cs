using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Services;
using DanceConventionClient.Views.RegistrationDesk;
using Xamarin.Forms;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace DanceConventionClient
{
	public partial class RegistrationDeskPage : ContentPage
	{
		public DanceEvent CurrentEvent { get; set; }
		private readonly IDCService _service;

		public RegistrationDeskPage(DanceEvent currentEvent)
		{
			InitializeComponent();
			ProfileItem.SetProfileButton(this);

			CurrentEvent = currentEvent;
			_service = App.MyService;
			Title = currentEvent.Name;
		}

		private async void OnSearchButtonPressed(object sender, EventArgs e)
		{
			var keyword = SearchBar.Text;
			var signups = await _service.SearchSignups(CurrentEvent.Id, keyword);
			var signupList = new SignupListView(signups);

			if (signupList.ItemsSource != null)
			{
				ContentStack.Children.Clear();
				ContentStack.Children.Add(signupList);
				signupList.ItemTapped += SignupListOnItemTapped;
			}
			else
			{
				InfoLabel.Text = "No Results";
			}			
		}

		private void SignupListOnItemTapped(object sender, ItemTappedEventArgs itemTappedEventArgs)
		{
			var signup = itemTappedEventArgs.Item as Signup;

			ContentStack.Children.Clear();
			ContentStack.Children.Add(new SignupView(CurrentEvent, signup));
		}

		private async void CameraButton_OnClicked(object sender, EventArgs e)
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
					await Navigation.PopAsync();

					await ShowSignup(result);
				});

			ContentStack.Children.Clear();
			await Navigation.PushAsync(new RegistrationCameraPage(zxing));
		}

		private async Task ShowSignup(Result result)
		{
			var elements = result.Text.Split('[', ':', ']');
			int eventId;
			int userId;

			if ((int.TryParse(elements[1], out eventId)) && (int.TryParse(elements[2], out userId)))
			{
				var signup = await _service.GetSignup(eventId, userId);
				ContentStack.Children.Add(new SignupView(CurrentEvent, signup));
			}
		}
	}
}
