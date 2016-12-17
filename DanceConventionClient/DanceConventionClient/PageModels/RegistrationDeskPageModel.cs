using System;
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
	[ImplementPropertyChanged]
	public class RegistrationDeskPageModel:FreshMvvm.FreshBasePageModel
	{
		private readonly IDCService _service;
		public DanceEvent CurrentEvent { get; set; }
		public bool ShowInfo { get; set; }
		public string InfoText { get; set; }

		public string Text { get; set; }
		public List<Signup> SignupList { get; set; }
		public bool ShowSignupList { get; set; }

		public Signup CurrentSignup { get; set; }
		public bool ShowCurrentSignup { get; set; }
		public Color StatusColor { get; set; }

		public RegistrationDeskPageModel()
		{
			_service = App.MyService;
			SetVisibility(true, false, false);
			InfoText = "Enter participant name \n or scan QR code";
		}

		public override void Init(object initData)
		{
			base.Init(initData);
			CurrentEvent = initData as DanceEvent;
		}

		public void SetVisibility(bool info, bool list, bool table)
		{
			ShowInfo = info;
			ShowSignupList = list;
			ShowCurrentSignup = table;
		}

		public Command SearchCommand
		{
			get
			{
				return new Command(async (dEvent) => {
					var signups = await _service.SearchSignups(CurrentEvent.Id, Text);					
					if (signups.Length > 0)
					{
						SignupList = signups.ToList();
						SetVisibility(false, true, false);
						RaisePropertyChanged();
					}
					else
					{
						InfoText = "No Results";
						RaisePropertyChanged();
					}
				});
			}
		}

		public Signup SelectedItem
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

		public Command<Signup> SelectedItemCommand
		{
			get
			{
				return new Command<Signup>((signup) =>
				{
					CurrentSignup = signup;
					SetVisibility(false, false, true);
					GetStatusColor();
				});				
			}			
		}

		private void GetStatusColor()
		{
			if (CurrentSignup.Status == "ATTENDED" || CurrentSignup.Status == "CANCELLED" || CurrentSignup.Status == "APPROVED")
			{
				StatusColor = Color.Blue;
			}
			else if (CurrentSignup.Status == "PAID")
			{
				StatusColor = Color.Green;
			}
			else if (CurrentSignup.Status == "BOOKED")
			{
				StatusColor = Color.Teal;
			}
			else if (CurrentSignup.Status == "WAITLIST")
			{
				StatusColor = Color.Maroon;
			}
		}

		public Command TableTapCommand
		{
			get
			{
				return new Command(async () =>
				{
					var identifier = new SignupIdentifier{CurrentEvent = CurrentEvent, Participant = CurrentSignup};
					await CoreMethods.PushPageModel<UserRegistrationPageModel>(identifier);
				});
			}
		}

		public Command CheckinCommand
		{
			get
			{
				return new Command(async() =>
				{
					if (CurrentSignup.Attended)
					{
						var answer = await CoreMethods.DisplayAlert("Confirmation", "Do you really want to undo this check-in?", "Yes", "No");

						if (answer)
						{
							var signup = await _service.UpdateAttendanceStatus(CurrentEvent.Id, CurrentSignup.ParticipantId);
							CurrentSignup = signup;
							RaisePropertyChanged();
						}
					}
					else
					{
						var signup = await _service.UpdateAttendanceStatus(CurrentEvent.Id, CurrentSignup.ParticipantId);
						CurrentSignup = signup;
						RaisePropertyChanged();
					}

				});
			}
		}
	}
}
