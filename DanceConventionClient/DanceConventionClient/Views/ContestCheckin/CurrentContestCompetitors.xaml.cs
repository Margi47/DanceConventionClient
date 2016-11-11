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
			_service = App.MyService;
			CurrentContest = currentContest;

			InitializeList();
		}

		private async void InitializeList()
		{
			var competitors = await _service.GetCompetitors(CurrentContest.EventId, CurrentContest.CompetitionId);
			if (competitors != null)
			{
				CompetitorsList.ItemsSource = competitors;
			} 			
		}

		private async void CompetitorsSearch_OnSearchButtonPressed(object sender, EventArgs e)
		{
			var keyword = CompetitorsSearch.Text;
			var result = await _service.SearchCompetitor(CurrentContest.EventId, CurrentContest.CompetitionId, keyword);
			CompetitorsList.ItemsSource = result;
		}

		private void CompetitorsList_OnItemTapped(object sender, ItemTappedEventArgs e)
		{
			var competitor = e.Item as Competitor;
			_service.ContestCheckin(CurrentContest.EventId, CurrentContest.CompetitionId, competitor.ParticipantId,
				competitor.BibNumber, false);
			InitializeList();
		}
	}
}
