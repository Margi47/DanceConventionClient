using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Services;
using Xamarin.Forms;

namespace DanceConventionClient.PageModels
{
	public class CurrentContestCompetitorsPageModel:FreshMvvm.FreshBasePageModel
	{
		private readonly IDCService _service;
		public Contest CurrentContest { get; set; }
		public List<Competitor> Competitors { get; set; }

		public CurrentContestCompetitorsPageModel()
		{
			_service = App.MyService;
		}

		public override async void Init(object initData)
		{
			base.Init(initData);
			CurrentContest = initData as Contest;
			var competitors = await _service.GetCompetitors(CurrentContest.EventId, CurrentContest.CompetitionId);
			Competitors = competitors.ToList();
		}

		public Competitor SelectedCompetitor
		{
			get
			{
				return null;
			}
			set
			{
				SelectedItemCommand.Execute(value);
				RaisePropertyChanged();
			}
		}

		public Command<Competitor> SelectedItemCommand
		{
			get
			{
				return new Command<Competitor>(async (competitor) =>
				{
					await _service.ContestCheckin(CurrentContest.EventId, CurrentContest.CompetitionId, competitor.ParticipantId,
					competitor.BibNumber, false);
					var competitors = await _service.GetCompetitors(CurrentContest.EventId, CurrentContest.CompetitionId);
					Competitors = competitors.ToList();
				});
			}
		}


	}
}
