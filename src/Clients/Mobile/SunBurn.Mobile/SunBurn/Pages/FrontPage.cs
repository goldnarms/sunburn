using System;
using Xamarin.Forms;
using SunBurn.Managers;

namespace SunBurn.Pages
{
	public class FrontPage : ContentPage
	{
		public FrontPage (){
			var manager = new FrontPageManager (DependencyService.Get<ILocationManager>(), new DataService());
			var location = manager.GetCurrentLocation ();

			Content = new StackLayout {
				VerticalOptions = LayoutOptions.Center,
				Children = {
					new Label {
						XAlign = TextAlignment.Center,
						Text = "Welcome to Xamarin Forms!, your location is: " + location.Item1 + " - " + location.Item2
					}
				}
			};

		}
	}
}

