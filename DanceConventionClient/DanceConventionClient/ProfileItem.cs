using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.PageModels;
using DanceConventionClient.Pages;
using FreshMvvm;
using Newtonsoft.Json.Serialization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DanceConventionClient
{ 
	public class ProfileItem
	{
		public static void SetProfileButton(Page page)
		{

			if (page.ToolbarItems.Count == 0)
			{
				page.ToolbarItems.Add(new ToolbarItem
				{
					Icon = "ic_account_circle_black_48dp.png",
					Order = ToolbarItemOrder.Primary,
					Command = new Command(() =>
					{

						var pageRes = FreshPageModelResolver.ResolvePageModel<ProfilePageModel>();
						
						var container = new FreshNavigationContainer(pageRes);
						Device.BeginInvokeOnMainThread(async () =>
						{
							await container.PushPage(new ProfilePage(), new ProfilePageModel());
						});
					})
				});
			}
			
		}
	}
}
