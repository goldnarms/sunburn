using System;
using Xamarin.Forms;

namespace SunBurn
{
	public class App : Application
	{
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
			// Check if user has set skin type
			if(Settings.SkinTypeSetting == SkinType.NotSet)
				MainPage = new NavigationPage (new SetSkintypePage());
			else
				MainPage = new NavigationPage (new FrontPage());
		}
	}
}

