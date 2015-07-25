using System;
using Xamarin.Forms;
using SunBurn.Managers;
using SunBurn.Calculators;
using SunBurn.BLL;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace SunBurn
{
	public class HomePage : CarouselPage
	{
		private HomePageManager _manager;
		private BandService _bandService;
		private double _uvIndex;
		private Label _uvIndexLbl;

		public HomePage ()
		{
			//BindingContext = new HomeViewModel ();
			IsBusy = true;
			var positionService = DependencyService.Get<IPositionService> ();
			var dataService = new DataService ();
			var locationService = new LocationService (positionService, dataService);
			_manager = new HomePageManager (locationService, dataService, new ExposureCalculator());
			Init ();
			IsBusy = false;
		}


		public HomePage (ILocationService locationService, IDataService dataService){
			IsBusy = true;
			_manager = new HomePageManager (locationService, dataService, new ExposureCalculator());
			Init ();
			IsBusy = false;
		}

		private async void Init(){
			var location = _manager.GetCurrentLocation ();
			try {
				var result = await _manager.GetResult (location);
				if(_bandService.HasDevice){
					await _bandService.UpdateTile(result.SunburnResults.First().Value);
				}

				foreach (var page in result.SunburnResults.Select (sr => BuildContent (result.Location, sr.Key, sr.Value))) {
					Children.Add (page);
				};
			} catch (Exception ex) {
				Children.Clear ();
				Children.Add(BuildContent ("Location", DateTime.Now.Date.ToString("M"), new SunburnResult {
					Celcius = 0,
					Fahrenheit = 0,
					UvIndex = 0,
					SpfTable = new List<SpfTime> () {
						new SpfTime {
							Spf = 0,
							Time = TimeSpan.FromMinutes (30).ToString("c")
						},
						new SpfTime {
							Spf = 15,
							Time = TimeSpan.FromMinutes (60).ToString("c")
						}
					}
				}));
				this.DisplayAlert ("Error", ex.Message, "Ok");
			}
		}

		private async Task ConnectToMsBand(){
			_bandService = new BandService ();
			await _bandService.SetupDevice ();
		}

		private ContentPage BuildContent(string locationName, string time, SunburnResult sunburnResult){
			var locationLbl = new Label {
				Text = locationName,
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.StartAndExpand,
				Style = Styles.linkLabelStyle
			};

			var dateLbl = new Label {
				Text = time,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
				FontAttributes = FontAttributes.Bold

			};

			_uvIndexLbl = new Label {
				Text = _uvIndex.ToString(),
				HorizontalOptions = LayoutOptions.StartAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				FontSize = 96
			};

			var uvLayout = new StackLayout {
				Children = {
					new Label {
						Text = "UV Index",
						HorizontalOptions = LayoutOptions.EndAndExpand,
						VerticalOptions = LayoutOptions.CenterAndExpand
					},
					_uvIndexLbl
				},
				Orientation = StackOrientation.Horizontal,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Padding = new Thickness(0, 20),
				Style = Styles.backgroundLayoutStyle
			};

			var sunBurnTable = new ListView {
				ItemsSource = sunburnResult.SpfTable,
				Header = "",
				HeaderTemplate = new DataTemplate(() => {
					Label spfHeaderLbl = new Label{
						Text = "Spf",
						FontAttributes = FontAttributes.Bold,
						HorizontalOptions = LayoutOptions.CenterAndExpand,
						FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
					};

					Label timeHeaderLbl = new Label{
						Text = "Duration",
						FontAttributes = FontAttributes.Bold,
						HorizontalOptions = LayoutOptions.CenterAndExpand,
						FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
					};

					return new StackLayout{
						Orientation = StackOrientation.Horizontal,
						Children = { spfHeaderLbl, timeHeaderLbl},
						BackgroundColor = Color.Gray
					};
				}),
				ItemTemplate = new DataTemplate(() => {
					Label spfLbl = new Label{
						HorizontalOptions = LayoutOptions.CenterAndExpand,
						Style = Styles.infoLabelStyle
					};
					spfLbl.SetBinding(Label.TextProperty, "Spf");

					Label timeLbl = new Label{
						HorizontalOptions = LayoutOptions.CenterAndExpand,
						Style = Styles.infoLabelStyle
					};
					timeLbl.SetBinding(Label.TextProperty, "Time");

					return new ViewCell{						
						View = new StackLayout{
							Orientation = StackOrientation.Horizontal,
							VerticalOptions = LayoutOptions.CenterAndExpand,
							Spacing = 0,
							Padding = new Thickness(0, 5),
							Children = { spfLbl, timeLbl}
						}
					};
				}),
				VerticalOptions = LayoutOptions.End,
				RowHeight = 40,

				
			};
			var layout = new StackLayout {
				VerticalOptions = LayoutOptions.StartAndExpand,
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