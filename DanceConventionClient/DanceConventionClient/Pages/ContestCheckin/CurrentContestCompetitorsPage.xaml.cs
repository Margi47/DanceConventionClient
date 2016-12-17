using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Services;
using Xamarin.Forms;

namespace DanceConventionClient.Pages
{
	public partial class CurrentContestCompetitorsPage : ContentPage
	{
		public CurrentContestCompetitorsPage()
		{
			InitializeComponent();
			ProfileItem.SetProfileButton(this);			
		}
	}
}
