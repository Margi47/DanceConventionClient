using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

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
					Command = new Command(async () =>
					{
						await page.Navigation.PushAsync(new ProfilePage());
					})
				});
			}
			
		}
	}
}
