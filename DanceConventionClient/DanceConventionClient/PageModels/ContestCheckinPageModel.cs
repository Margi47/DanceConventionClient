using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Services;

namespace DanceConventionClient.PageModels
{
	public class ContestCheckinPageModel:FreshMvvm.FreshBasePageModel
	{
		private readonly IDCService _service;
		public DanceEvent CurrentEvent { get; set; }
		public List<Contest> ContestsList { get; set; }

		public bool ShowInfo { get; set; }
		public bool ShowList { get; set; }
		public bool CameraEnabled { get; set; }

		public ContestCheckinPageModel()
		{
			_service = App.MyService;
		}

		public override async void Init(object initData)
		{
			base.Init(initData);
			CurrentEvent = initData as DanceEvent;

			var contests = await _service.GetContests(CurrentEvent.Id);
			if (contests.Length > 0)
			{
				ContestsList = contests.ToList();
				SetVisibility(true, true, false);				
			}
			else
			{
				SetVisibility(false, false, true);
			}

			RaisePropertyChanged();
		}

		public void SetVisibility(bool camera, bool list, bool info)
		{
			CameraEnabled = camera;
			ShowList = list;
			ShowInfo = info;
		}

		public Contest SelectedContest
		{
			get
			{
				return null;
			}
			set
			{
				CoreMethods.PushPageModel<CurrentContestCompetitorsPageModel>(value);
				RaisePropertyChanged();
			}
		}
	}
}
