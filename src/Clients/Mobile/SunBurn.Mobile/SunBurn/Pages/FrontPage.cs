using System;
using Xamarin.Forms;
using SunBurn.Managers;
using SunBurn.Calculators;
using System.Threading.Tasks;
using System.Linq;

namespace SunBurn
{
	public class FrontPage : ContentPage
	{
		private FrontPageManager _manager;
		public FrontPage (){
			_manager = new FrontPageManager (DependencyService.Get<ILocationManager>(), new DataService(), new ExposureCalculator());
			var location = _manager.GetCurrentLocation ();
			var result = _manager.GetResult (location);

			Content = BuildContent(result.Location, result.SunburnResults.First().Key, result.SunburnResults.First().Value);
			Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);
		}

		private View BuildContent(string locationName, DateTime time, SunburnResult sunburnResult){
			var locationLbl = new Label {
				Text = locationName,
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.StartAndExpand
			};

			var dateLbl = new Label {
				Text = time.ToString ("dddd"),
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};

			var uvLayout = new StackLayout {
				Children = {
					new Label {
						Text = "UV Index",
						HorizontalOptions = LayoutOptions.CenterAndExpand
					},
					new Label {
						Text = sunburnResult.UvIndex.ToString(),
						HorizontalOptions = LayoutOptions.CenterAndExpand
					}
				},
				Orientation = StackOrientation.Horizontal
			};

			var sunBurnTable = new ListView {
				ItemsSource = sunburnResult.SpfTable,
				HeaderTemplate = new DataTemplate(() => {
					Label spfHeaderLbl = new Label{
						Text = "Spf"
					};

					Label timeHeaderLbl = new Label{
						Text = "Duration"
					};

					return new ViewCell{
						View = new StackLayout{
							Orientation = StackOrientation.Horizontal,
							Children = { spfHeaderLbl, timeHeaderLbl}
						}
					};
				}),
				ItemTemplate = new DataTemplate(() => {
					Label spfLbl = new Label();
					spfLbl.SetBinding(Label.TextProperty, "Spf");

					Label timeLbl = new Label();
					timeLbl.SetBinding(Label.TextProperty, "Time");

					return new ViewCell{
						View = new StackLayout{
							Orientation = StackOrientation.Horizontal,
							Children = { spfLbl, timeLbl}
						}
					};
				})
			};
			return new StackLayout {
				VerticalOptions = LayoutOptions.Center,
				Children = {
					locationLbl, uvLayout, sunBurnTable
				}
			};
		}
	}
}