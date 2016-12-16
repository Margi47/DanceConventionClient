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
		public Signup CurrentSignup { get; set; }
		public DanceEvent CurrentEvent { get; set; }
		public Color TextColor { get; set; }
		public SignupIdentifier Identifier { get; set; }

		public override void Init(object initData)
		{
			base.Init(initData);
			Identifier = initData as SignupIdentifier;
			CurrentEvent = Identifier.Event;
			CurrentSignup = Identifier.Participant;

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
