using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Services;
using Xamarin.Forms;

namespace DanceConventionClient
{
	public partial class SignupListView : ListView
	{
		public DanceEvent CurrentEvent { get; set; }
		public string Keyword { get; set; }
		private readonly IDCService _service;

		public SignupListView(Signup[] signups)
		{
			InitializeComponent();
			_service = App.MyService;
			//CurrentEvent = danceEvent;
			//Keyword = keyword;
			//InitListInfo();

			ItemsSource = signups;
		}

		private async void InitListInfo()
		{
			ItemsSource = await _service.SearchSignups(CurrentEvent.Id, Keyword);
		}
	}
}
