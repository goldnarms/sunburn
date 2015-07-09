using System;
using Xamarin.Forms;

namespace SunBurn
{
	public class RootPage : MasterDetailPage
	{
		MenuPage menuPage;
		public RootPage ()
		{
			menuPage = new MenuPage ();
			MasterBehavior = MasterBehavior.Popover;
			IsGestureEnabled = false;
			menuPage.Menu.ItemSelected += (object sender, SelectedItemChangedEventArgs e) => NavigateTo(e.SelectedItem as MenuItem);

			Master = menuPage;

			// Check if user has set skin type
			if(Settings.SkinTypeSetting == SkinType.NotSet)
				Detail = new NavigationPage (new SetSkintypePage());
			else
				Detail = new NavigationPage (new FrontPage());
		}

		private void NavigateTo(MenuItem menuItem){
			if (menuItem == null)
				return;
			
			Page displayPage = (Page)Activator.CreateInstance(menuItem.TargetType);
			Detail = new NavigationPage (displayPage);
			menuPage.Menu.SelectedItem = null;
			IsPresented = false;
		}
	}
}

