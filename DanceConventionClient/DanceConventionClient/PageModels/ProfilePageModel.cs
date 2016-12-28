using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Services;
using Xamarin.Forms;

namespace DanceConventionClient.PageModels
{
	public class ProfilePageModel : FreshMvvm.FreshBasePageModel
	{
		private readonly IDCService _service;
		public Profile CurrentProfile { get; set; }
		public bool IsLoading { get; set; }

		public ProfilePageModel()
		{
			_service = App.MyService;
		}

		public override async void Init(object initData)
		{
			IsLoading = true;
			base.Init(initData);
			CurrentProfile = await _service.GetProfile();
			IsLoading = false;
		}

		public Command LogoutCommand
		{
			get
			{
				return new Command(() =>
				{
					Application.Current.Properties.Remove("userName");
					Application.Current.Properties.Remove("password");

					App.NavigateToLoginPage();
				});
			}
		}
	}
}
