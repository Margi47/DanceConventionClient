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
	public class LoginPageModel : FreshMvvm.FreshBasePageModel
	{
		public string UserName { get; set; }
		public string UserPassword { get; set; }
		public bool IsLoading { get; set; }

		public Command LoginCommand
		{
			get
			{
				return new Command(async () =>
				{
					var login = new DCLogin {Username = UserName, Password = UserPassword};
					var service = new DCServiceWrapper();

					IsLoading = true;
					var loginResult = await service.Login(login);
					IsLoading = false;

					if (loginResult == null)
					{
						return;
					}
					

					if (loginResult.Successful)
					{
						await App.InitializeService(loginResult, service);
						App.NavigateToMainPage();
					}
					else
					{
						await CoreMethods.DisplayAlert("Error", loginResult.ErrorMessage, "OK");
					}

				});
			}
		}

		public Command SettingsCommand
		{
			get
			{
				return new Command(async () =>
				{
					await CoreMethods.PushPageModel<SettingsPageModel>();
				});
			}
		}
	}
}
