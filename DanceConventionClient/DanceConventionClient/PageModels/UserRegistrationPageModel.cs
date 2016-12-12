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
	public class UserRegistrationPageModel:FreshMvvm.FreshBasePageModel
	{
		private readonly IDCService _service;
		public Signup CurrentSignup { get; set; }
		public DanceEvent CurrentEvent { get; set; }
		public Color TextColor { get; set; }
		public SignupIdentifier Identifier { get; set; }

		public UserRegistrationPageModel()
		{
			_service = App.MyService;
		}

		public override async void Init(object initData)
		{
			base.Init(initData);
			CurrentEvent = initData as DanceEvent;

			var profile = await _service.GetProfile();
			CurrentSignup = await _service.GetSignup(CurrentEvent.Id, profile.Id);

			Identifier = new SignupIdentifier() { EventId = CurrentEvent.Id, ParticipantId = CurrentSignup.ParticipantId };

			TextColor = CurrentSignup.AmountPaid >= CurrentSignup.AmountInvoiced ? Color.Blue : Color.Red;
		}

		public Command QrCodeCommand
		{
			get
			{
				return new Command(async (signup) => {
					await CoreMethods.PushPageModel<BarcodePageModel>(Identifier);
				});
			}
		}
	}
}
