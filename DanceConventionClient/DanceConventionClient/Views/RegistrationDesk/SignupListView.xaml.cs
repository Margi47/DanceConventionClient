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
		private readonly IDCService _service;

		public SignupListView(Signup[] signups)
		{
			InitializeComponent();
			_service = App.MyService;

			ItemsSource = signups;
		}
	}
}
