﻿using System;
using Xamarin.Forms;
using Connectivity.Plugin;
using SunBurn.BLL;

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
			var deviceService = DependencyService.Get<IDeviceService> ();
			//deviceService.SetupSettings ();
				
			page = MainPage = new RootPage ();

			CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
			{
				if(!args.IsConnected)
					page.DisplayAlert("Connect to internet", "Please connect to internet", "OK");
			};
		}


	}
}

