using System;
using Xamarin.Forms;
using SunBurn.Managers;
using SunBurn.Calculators;
using System.Threading.Tasks;
using System.Linq;

namespace SunBurn
{
	public class FrontPage : CarouselPage
	{
		private FrontPageManager _manager;
		public FrontPage (ILocationService locationService, IDataService dataService){
			_manager = new FrontPageManager (locationService, dataService, new ExposureCalculator());
			Init ();
		}

		private async void Init(){
			var location = _manager.GetCurrentLocation (); // Set later in the lifecyclee
			var result = await _manager.GetResult (location);
			foreach (var page in result.SunburnResults.Select (sr => BuildContent (result.Location, sr.Key, sr.Value))) {
				Children.Add (page);
			};
		}

		private ContentPage BuildContent(string locationName, string time, SunburnResult sunburnResult){
			var locationLbl = new Label {
				Text = locationName,
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.StartAndExpand,
				FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
			};

			var dateLbl = new Label {
				Text = time,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),

			};

			var uvLayout = new StackLayout {
				Children = {
					new Label {
						Text = "UV Index",
						HorizontalOptions = LayoutOptions.CenterAndExpand
					},
					new Label {
						Text = sunburnResult.UvIndex.ToString(),
						HorizontalOptions = LayoutOptions.CenterAndExpand,
						FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
					}
				},
				Orientation = StackOrientation.Horizontal,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			var sunBurnTable = new ListView {
				ItemsSource = sunburnResult.SpfTable,

				HeaderTemplate = new DataTemplate(() => {
					Label spfHeaderLbl = new Label{
						Text = "Spf",
						FontAttributes = FontAttributes.Bold,
						HorizontalOptions = LayoutOptions.CenterAndExpand
					};

					Label timeHeaderLbl = new Label{
						Text = "Duration",
						FontAttributes = FontAttributes.Bold,
						HorizontalOptions = LayoutOptions.CenterAndExpand
					};

					return new StackLayout{
						Orientation = StackOrientation.Horizontal,
						Children = { spfHeaderLbl, timeHeaderLbl}
					};
				}),
				ItemTemplate = new DataTemplate(() => {
					Label spfLbl = new Label{
						HorizontalOptions = LayoutOptions.CenterAndExpand,

					};
					spfLbl.SetBinding(Label.TextProperty, "Spf");

					Label timeLbl = new Label{
						HorizontalOptions = LayoutOptions.CenterAndExpand,
					};
					timeLbl.SetBinding(Label.TextProperty, "Time");

					return new ViewCell{
						View = new StackLayout{
							Orientation = StackOrientation.Horizontal,
							VerticalOptions = LayoutOptions.CenterAndExpand,
							Children = { spfLbl, timeLbl}
						}
					};
				}),
				VerticalOptions = LayoutOptions.End
			};
			var layout = new StackLayout {
				Padding = new Thickness(20),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					locationLbl, dateLbl, uvLayout, sunBurnTable
				}
			};

			return new ContentPage {
				Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5),
				Content = layout
			};
		}
	}
}