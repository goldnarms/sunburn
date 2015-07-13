using System;
using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SunBurn
{
	public class MenuListView:ListView
	{
		public MenuListView ()
		{
			List<MenuItem> data = new MenuListData ();

			ItemsSource = data;
			VerticalOptions = LayoutOptions.FillAndExpand;
			BackgroundColor = Color.Transparent;
			SeparatorVisibility = SeparatorVisibility.None;

			var cell = new DataTemplate (typeof(ImageCell));
			cell.SetBinding (TextCell.TextProperty, "Title");
			cell.SetBinding (ImageCell.ImageSourceProperty, "IconSource");
			
			ItemTemplate = cell;
		}
	}
}

