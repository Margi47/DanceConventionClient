using DanceConventionClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DanceConventionClient
{
	public partial class MyEventspage : ContentPage
	{
		public IDCService Service = App.MyService;

		public MyEventspage()
		{
			InitializeComponent();
			
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await InitializeEvents();
		}

		private async Task InitializeEvents()
		{
			var allEvents = await Service.GetEvents();
			var curEventSource = new List<DanceEvent>();
			var pastEventsSource = new List<DanceEvent>();
			var curTime = DateTime.Now.Date;

			foreach(DanceEvent ev in allEvents)
			{
				if (ev.EndDate >= curTime)
				{
					curEventSource.Add(ev);
				}
				else
				{
					pastEventsSource.Add(ev);
				}
			}

			currentEvents.ItemsSource = curEventSource;
			pastEvents.ItemsSource = pastEventsSource;
		}

		private async void events_ItemTapped(object sender, ItemTappedEventArgs e)
		{				
			await Navigation.PushAsync(new CurrentEventPage(e.Item as DanceEvent));	
		}
	}
}
