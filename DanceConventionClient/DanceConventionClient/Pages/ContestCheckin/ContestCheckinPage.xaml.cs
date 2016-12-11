﻿using System;
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

namespace DanceConventionClient
{
	public partial class ContestCheckinPage : ContentPage
	{
		private readonly IDCService _service;
		public DanceEvent CurrentEvent { get; set; }

		public ContestCheckinPage(DanceEvent currentEvent)
		{
			InitializeComponent();
			ProfileItem.SetProfileButton(this);

			CurrentEvent = currentEvent;
			_service = App.MyService;

			Title = currentEvent.Name;
			GetContests();
		}

		private async void GetContests()
		{
			var contests = await _service.GetContests(CurrentEvent.Id);
			if (contests.Length > 0)
			{
				Device.BeginInvokeOnMainThread((() =>
				{
					ContestsList.ItemsSource = contests;
					ChangeVisibility();
				}));			
			}			 
		}

		private void ChangeVisibility()
		{
			Device.BeginInvokeOnMainThread((() =>
			{
				ContestsList.IsVisible = true;
				NoResultLabel.IsVisible = false;
				CameraButton.IsEnabled = true;
			}));		
		}


		private async void Camera_OnClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new CameraPage() {FirstCall = true});
		}


		private async void ContestsList_OnItemTapped(object sender, ItemTappedEventArgs e)
		{
			await Navigation.PushAsync(new CurrentContestCompetitors(e.Item as Contest));
		}
	}
}