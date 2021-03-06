﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Services;
using DanceConventionClient.Services.Models;
using PropertyChanged;
using Xamarin.Forms;

namespace DanceConventionClient.PageModels
{
	public class CurrentEventPageModel : FreshMvvm.FreshBasePageModel
	{
		private readonly IDCService _service;
		private SignupIdentifier _identifier;
		public DanceEvent CurrentEvent { get; set; }
		public bool IsAllStaff { get; set; }
		public bool IsRegDeskStaff { get; set; }
		public bool IsCompetitor { get; set; }
		public bool IsLoading { get; set; }

		public CurrentEventPageModel()
		{
			_service = App.MyService;
		}

		public override async void Init(object initData)
		{
			IsLoading = true;
			base.Init(initData);
			CurrentEvent = initData as DanceEvent;
			var profile = await _service.GetProfile();
			if (profile == null)
			{
				return;
			}

			var signup = await _service.GetSignup(CurrentEvent.Id, profile.Id);
			if (signup == null)
			{
				return;
			}

			_identifier = new SignupIdentifier {CurrentEvent = CurrentEvent, Participant = signup};
			await InitializeButtons();
			IsLoading = false;
		}


		private async Task InitializeButtons()
		{
			var permissions = await _service.GetPermissions(CurrentEvent.Id);
			if (permissions == null)
			{
				return;
			}

			var allPermissions = new string[]
				{"EVENT_OWNER", "STAFF_EVENT_ADMIN",   "STAFF_SCORES", "STAFF_CONTEST_CHECKIN"};
			var regDeskPermissions = new string[]
				{"STAFF_REGDESK", "STAFF_FINANCE"};
			var signupPermissions = new string[]
				{"SIGNED_UP_FOR_EVENT", "INVOICE_OWNER", "SIGNUP_HOST", "SIGNUP_OWNER"};

			IsAllStaff = permissions.Any(p => allPermissions.Contains(p.Permission));
			IsRegDeskStaff = permissions.Any(p => regDeskPermissions.Contains(p.Permission))||
				permissions.Any(p => allPermissions.Contains(p.Permission));
			IsCompetitor = permissions.Any(p => signupPermissions.Contains(p.Permission));
		}

		public Command<SignupIdentifier> MyRegistrationCommand
		{
			get
			{
				return new Command<SignupIdentifier>(async (user) => {
					await CoreMethods.PushPageModel<UserRegistrationPageModel>(_identifier);
				});
			}
		}

		public Command<DanceEvent> ContestCheckinCommand
		{
			get
			{
				return new Command<DanceEvent>(async (dEvent) => {
					await CoreMethods.PushPageModel<ContestCheckinPageModel>(CurrentEvent);
				});
			}
		}

		public Command<DanceEvent> RegistrationCommand
		{
			get
			{
				return new Command<DanceEvent>(async (dEvent) => {
					await CoreMethods.PushPageModel<RegistrationDeskPageModel>(CurrentEvent);
				});
			}
		}
	}
}
