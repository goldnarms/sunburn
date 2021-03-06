﻿using System;
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
			var positionService = DependencyService.Get<IPositionService> ();
			var dataService = new DataService ();
			var locationService = new LocationService (positionService, dataService);
				
			// Check if user has set skin type
			if(Settings.SkinTypeSetting == SkinType.NotSet)
				Detail = new NavigationPage (new SettingsPage(locationService, dataService));
			else
				Detail = new NavigationPage (new HomePage(locationService, dataService));
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

