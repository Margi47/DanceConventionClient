using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Pages;
using DanceConventionClient.Services;
using DanceConventionClient.Services.Models;
using PropertyChanged;
using Xamarin.Forms;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace DanceConventionClient.PageModels
{
	[ImplementPropertyChanged]
	public class RegistrationDeskPageModel : FreshMvvm.FreshBasePageModel
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

		public decimal PaymentAmount { get; set; }
		public string PaymentComment { get; set; }

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
				return new Command(async (dEvent) =>
				{
					var signups = await _service.SearchSignups(CurrentEvent.Id, Text);
					if (signups == null)
					{
						return;
					}

					if (signups.Length > 0)
					{
						SignupList = signups.ToList();
						SetVisibility(false, true, false);
					}
					else
					{
						InfoText = "No Results";
					}
				});
			}
		}

		public Signup SelectedItem
		{
			get { return null; }
			set { SelectedItemCommand.Execute(value); }
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
					PaymentAmount = CurrentSignup.AmountOwed;
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
					var identifier = new SignupIdentifier {CurrentEvent = CurrentEvent, Participant = CurrentSignup};
					await CoreMethods.PushPageModel<UserRegistrationPageModel>(identifier);
				});
			}
		}

		public Command CheckinCommand
		{
			get
			{
				return new Command(async () =>
				{
					if (CurrentSignup.Attended)
					{
						var answer =
							await CoreMethods.DisplayAlert("Confirmation", "Do you really want to undo this check-in?", "Yes", "No");

						if (answer)
						{
							var signup = await _service.UpdateAttendanceStatus(CurrentEvent.Id, CurrentSignup.ParticipantId);
							if (signup == null)
							{
								return;
							}

							CurrentSignup = signup;
						}
					}
					else
					{
						var signup = await _service.UpdateAttendanceStatus(CurrentEvent.Id, CurrentSignup.ParticipantId);
						if (signup == null)
						{
							return;
						}

						CurrentSignup = signup;
					}
				});
			}
		}

		public Command PaymentCommand
		{
			get
			{
				return new Command(async () =>
				{
					if (PaymentAmount > 0)
					{
						var signup =
							await _service.RecordPayment(CurrentEvent.Id, CurrentSignup.ParticipantId, PaymentAmount, PaymentComment);
						if (signup == null)
						{
							return;
						}

						CurrentSignup = signup;
						PaymentAmount = CurrentSignup.AmountOwed;
					}
					else
					{
						await CoreMethods.DisplayAlert("Error", "Please enter payment amount", "OK");
					}
				});
			}
		}

		public Command ScannerCommand
		{
			get
			{
				return new Command(async () =>
				{
					var zxing = new ZXingScannerView
					{
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand
					};

					zxing.OnScanResult += (result) =>
						Device.BeginInvokeOnMainThread(async () =>
						{
							zxing.IsAnalyzing = false;
							await CoreMethods.PopPageModel();
							await ShowSignup(result);
						});

					await CoreMethods.PushPageModel<RegistrationCameraPageModel>(zxing);
				});
			}
		}

		private async Task ShowSignup(Result result)
		{
			var elements = result.Text.Split('[', ':', ']');
			int eventId;
			int userId;

			if ((int.TryParse(elements[1], out eventId)) && (int.TryParse(elements[2], out userId)))
			{
				var signup = await _service.GetSignup(eventId, userId);
				CurrentSignup = signup;
				SetVisibility(false, false, true);
				GetStatusColor();
				PaymentAmount = CurrentSignup.AmountOwed;
			}
		}
	}
}
