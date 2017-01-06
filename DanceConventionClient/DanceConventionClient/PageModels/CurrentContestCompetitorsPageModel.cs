using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Services;
using PropertyChanged;
using Xamarin.Forms;

namespace DanceConventionClient.PageModels
{
	[ImplementPropertyChanged]
	public class CurrentContestCompetitorsPageModel : FreshMvvm.FreshBasePageModel
	{
		private readonly IDCService _service;
		public Contest CurrentContest { get; set; }
		public List<Competitor> Competitors { get; set; }
		public bool IsLoading { get; set; }

		public string Text { get; set; }
		public string InfoText { get; set; }

		public bool ShowInfo { get; set; }
		public bool ShowList { get; set; }

		public CurrentContestCompetitorsPageModel()
		{
			_service = App.MyService;
			InfoText = AppResources.ContestPageInfo;
		}

		public override async void Init(object initData)
		{
			base.Init(initData);

			CurrentContest = initData as Contest;
			IsLoading = true;
			var competitors = await _service.GetCompetitors(CurrentContest.EventId, CurrentContest.CompetitionId);
			IsLoading = false;

			if (competitors == null)
			{
				return;
			}

			if (competitors.Length > 0)
			{
				Competitors = competitors.ToList();
				SetVisibility(true, false);
			}
			else
			{
				SetVisibility(false, true);
			}
		}

		private void SetVisibility(bool list, bool info)
		{
			ShowList = list;
			ShowInfo = info;
		}

		public Competitor SelectedCompetitor
		{
			get { return null; }
			set { SelectedItemCommand.Execute(value); }
		}

		public Command<Competitor> SelectedItemCommand
		{
			get
			{
				return new Command<Competitor>(async (competitor) =>
				{
					IsLoading = true;
					var result = await _service.ContestCheckin(CurrentContest.EventId, CurrentContest.CompetitionId, competitor.ParticipantId,
						competitor.BibNumber, false);
					if (result == null)
					{
						return;
					}

					var competitors = await _service.GetCompetitors(CurrentContest.EventId, CurrentContest.CompetitionId);
					if (competitors == null)
					{
						return;
					}

					IsLoading = false;

					Competitors = competitors.ToList();
				});
			}
		}

		public Command SearchCommand
		{
			get
			{
				return new Command(async () =>
				{
					IsLoading = true;
					var competitors = await _service.SearchCompetitor(CurrentContest.EventId, CurrentContest.CompetitionId, Text);
					IsLoading = false;

					if (competitors == null)
					{
						return;
					}

					if (competitors.Length > 0)
					{
						Competitors = competitors.ToList();
						SetVisibility(true, false);
					}
					else
					{
						SetVisibility(false, true);
					}
				});
			}
		}
	}
}
