using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Services;
using Xamarin.Forms;

namespace DanceConventionClient.PageModels
{
	public class MyEventsPageModel:FreshMvvm.FreshBasePageModel
	{
		private readonly IDCService _service;
		public List<DanceEvent> CurretntEvents { get; set; }
		public List<DanceEvent> PastEvents { get; set; }

		public MyEventsPageModel()
		{
			_service = App.MyService;
		}

		public override async void Init(object initData)
		{
			base.Init(initData);
			await InitializeEvents();		
		}

		private async Task InitializeEvents()
		{
			var curEv = new List<DanceEvent>();
			var pastev = new List<DanceEvent>();
			var allEvents = await _service.GetEvents();
			var curTime = DateTime.Now;

			foreach (var ev in allEvents)
			{
				if (ev.EndDate >= curTime)
				{
					curEv.Add(ev);
				}
				else
				{
					pastev.Add(ev);
				}
			}

			CurretntEvents = curEv;
			PastEvents = pastev;
		}

		public DanceEvent SeectedEvent
		{
			get
			{
				return null;
			}
			set
			{
				CoreMethods.PushPageModel<CurrentEventPageModel>(value);
				RaisePropertyChanged();
			}
		}
	}
}
