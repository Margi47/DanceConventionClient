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
	public class CurrentEventPageModel:FreshMvvm.FreshBasePageModel
	{
		private readonly IDCService _service;

		public DanceEvent CurrentEvent { get; set; }		
		public bool IsStaff { get; set; }
		public bool IsCompetitor { get; set; }

		public CurrentEventPageModel()
		{
			_service = App.MyService;
		}

		public override async void Init(object initData)
		{
			base.Init(initData);
			CurrentEvent = initData as DanceEvent;
			await InitializeButtons();
		}


		private async Task InitializeButtons()
		{
			var permissions = await _service.GetPermissions(CurrentEvent.Id);

			var staffPermissions = new string[]
				{"EVENT_OWNER", "STAFF_EVENT_ADMIN", "STAFF_REGDESK", "STAFF_FINANCE", "STAFF_SCORES", "STAFF_CONTEST_CHECKIN"};
			var signupPermissions = new string[]
				{"SIGNED_UP_FOR_EVENT", "INVOICE_OWNER", "SIGNUP_HOST", "SIGNUP_OWNER"};

			IsStaff = permissions.Any(p => staffPermissions.Contains(p.Permission));
			IsCompetitor = permissions.Any(p => signupPermissions.Contains(p.Permission));
		}

		public Command MyRegistrationCommand
		{
			get
			{
				return new Command(async (dEvent) => {
					await CoreMethods.PushPageModel<UserRegistrationPageModel>(CurrentEvent);
				});
			}
		}

		/*public Command<DanceEvent> ContactSlected
		{
			get
			{
				return new Command<DanceEvent>(async (dEvent) => {
					await CoreMethods.PushPageModel<CurrentEventPageModel>(dEvent);
				});
			}
		}

		public Command<DanceEvent> ContactSelected
		{
			get
			{
				return new Command<DanceEvent>(async (dEvent) => {
					await CoreMethods.PushPageModel<CurrentEventPageModel>(dEvent);
				});
			}
		}*/
	}
}
