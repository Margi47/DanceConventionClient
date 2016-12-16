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
	public class RegistrationDeskPageModel:FreshMvvm.FreshBasePageModel
	{
		private readonly IDCService _service;
		public DanceEvent CurrentEvent { get; set; }
		public bool ShowInfo { get; set; }

		public string Text { get; set; }
		public List<Signup> SignupList { get; set; }
		public bool ShowSignupList { get; set; }

		public Signup CurrentSignup { get; set; }
		public bool ShowCurrentSignup { get; set; }

		public RegistrationDeskPageModel()
		{
			_service = App.MyService;
			SetVisibility(true, false, false);
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
					SignupList = signups.ToList();
					SetVisibility(false, true, false);
					RaisePropertyChanged();
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
				});				
			}			
		}

	}
}
