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
	public class MyEventsPageModel : FreshMvvm.FreshBasePageModel
	{
		private readonly IDCService _service;
		public List<DanceEvent> CurrentEvents { get; set; }
		public List<DanceEvent> PastEvents { get; set; }
		public Color BackgroundColor { get; set; }
		public bool IsLoading { get; set; }

		public MyEventsPageModel()
		{
			_service = App.MyService;
		}

		public override async void Init(object initData)
		{
			base.Init(initData);
			BackgroundColor = Color.Transparent;
			await InitializeEvents();
		}

		private async Task InitializeEvents()
		{
			IsLoading = true;
			var currentEvents = new List<DanceEvent>();
			var pastEvents = new List<DanceEvent>();
			var allEvents = await _service.GetEvents();
			if (allEvents == null)
			{
				return;
			}

			var currentTime = DateTime.Now;

			foreach (var ev in allEvents)
			{
				if (ev.EndDate >= currentTime)
				{
					currentEvents.Add(ev);
				}
				else
				{
					pastEvents.Add(ev);
				}
			}

			CurrentEvents = currentEvents;
			PastEvents = pastEvents;

			IsLoading = false;
		}

		public DanceEvent SelectedEvent
		{
			get { return null; }
			set
			{
				CoreMethods.PushPageModel<CurrentEventPageModel>(value);
				RaisePropertyChanged();
			}
		}
	}
}
