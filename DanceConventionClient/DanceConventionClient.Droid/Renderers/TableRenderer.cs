using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DanceConventionClient.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ListView = Android.Widget.ListView;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(TableView), typeof(TableRenderer))]
namespace DanceConventionClient.Droid.Renderers
{	
	public class TableRenderer:TableViewRenderer
	{
		protected override Xamarin.Forms.Platform.Android.TableViewModelRenderer GetModelRenderer(ListView listView, TableView view)
		{
			return new MyTableViewModelRenderer(Context, listView, view);
		}


		class MyTableViewModelRenderer : Xamarin.Forms.Platform.Android.TableViewModelRenderer
		{
			//Info on private base class method
			private static readonly MethodInfo CellForPosition = typeof(Xamarin.Forms.Platform.Android.TableViewModelRenderer)
				.GetMethod(nameof(GetCellForPosition), BindingFlags.NonPublic | BindingFlags.Instance, null, CallingConventions.Any,
					new[] {typeof(int), typeof(bool).MakeByRefType(), typeof(bool).MakeByRefType()}, null);


			private readonly TableView _view;

			public MyTableViewModelRenderer(Context context, ListView listView, TableView view)
				: base(context, listView, view)
			{
				_view = view;
			}

			public override View GetView(int position, View convertView, ViewGroup parent)
			{
				var cellView = base.GetView(position, convertView, parent);

				if (_view.Intent == TableIntent.Data)
				{
					var @params = new object[] {position, false, false};
					var cell = CellForPosition.Invoke(this, @params) as Cell; //Reflection to base method to determine cell type

					bool shouldHide = (bool) @params[1] /*out isHeader*/
					                  && string.IsNullOrEmpty((cell as TextCell)?.Text);

					//With view recycling, we need to ensure Visibility is set to the proper value
					var visibility = shouldHide
						? ViewStates.Gone
						: ViewStates.Visible;

					cellView.Visibility = visibility;

					var cellGroup = cellView as ViewGroup;
					if (cellGroup != null)
					{
						foreach (var child in Enumerable.Range(0, cellGroup.ChildCount).Select(cellGroup.GetChildAt))
							child.Visibility = visibility;
					}
				}
				return cellView;
			}
		}
	}
}