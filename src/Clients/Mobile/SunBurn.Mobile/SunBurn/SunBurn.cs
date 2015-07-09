using System;
using Xamarin.Forms;
using Connectivity.Plugin;

namespace SunBurn
{
	public class App : Application
	{
		Page page;
		public App ()
		{
			// The root page of your application

			AppStart ();
		}

		protected override void OnStart ()
		{
			//var locationManager = S
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

		private void AppStart(){
//			MainPage = new ContentPage {
//				Content = new Label {
//					Text = "Test"
//				}
//			};
			// Check if user has set skin type
			if(Settings.SkinTypeSetting == SkinType.NotSet)
				page = MainPage = new NavigationPage (new SetSkintypePage());
			else
				page = MainPage = new NavigationPage (new FrontPage());

			CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
			{
				if(!args.IsConnected)
					page.DisplayAlert("Connect to internet", "Please connect to internet", "OK");
			};
		}


	}
}

