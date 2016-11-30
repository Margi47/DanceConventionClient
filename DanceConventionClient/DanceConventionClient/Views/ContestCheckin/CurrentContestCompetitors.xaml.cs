using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Services;
using Xamarin.Forms;

namespace DanceConventionClient
{
	public partial class CurrentContestCompetitors : ContentPage
	{
		private readonly IDCService _service;
		public Contest CurrentContest { get; set; }

		public CurrentContestCompetitors(Contest currentContest)
		{
			InitializeComponent();
			ProfileItem.SetProfileButton(this);

			_service = App.MyService;
			CurrentContest = currentContest;
			Title = currentContest.Name;			
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			await InitializeList();
		}

		private async Task InitializeList()
		{
			var competitors = await _service.GetCompetitors(CurrentContest.EventId, CurrentContest.CompetitionId);
			if (competitors != null)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					CompetitorsList.ItemsSource = competitors;
				});
			} 			
		}

		private async void CompetitorsSearch_OnSearchButtonPressed(object sender, EventArgs e)
		{
			var keyword = CompetitorsSearch.Text;
			var result = await _service.SearchCompetitor(CurrentContest.EventId, CurrentContest.CompetitionId, keyword);
			CompetitorsList.ItemsSource = result;
		}

		private async void CompetitorsList_OnItemTapped(object sender, ItemTappedEventArgs e)
		{
			var competitor = e.Item as Competitor;
			await _service.ContestCheckin(CurrentContest.EventId, CurrentContest.CompetitionId, competitor.ParticipantId,
				competitor.BibNumber, false);
			await InitializeList();
		}
	}
}
