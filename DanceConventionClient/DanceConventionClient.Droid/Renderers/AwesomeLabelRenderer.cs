using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using DanceConventionClient.Droid.Renderers;

[assembly: ExportRenderer(typeof(Label), typeof(AwesomeLabelRenderer))]

namespace DanceConventionClient.Droid.Renderers
{
	public class AwesomeLabelRenderer:LabelRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);

			if (Control.Text.Length == 0 || Control.Text.Length > 1 || Control.Text[0] < 0xf000)
			{
				return;
			}

			var font = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, "fontawesome-webfont.ttf");
			Control.Typeface = font;
		}
	}
}