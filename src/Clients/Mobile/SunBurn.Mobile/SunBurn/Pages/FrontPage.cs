using System;
using Xamarin.Forms;
using SunBurn.Managers;
using SunBurn.Calculators;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.Band.Portable;
using Microsoft.Band.Portable.Tiles;
using Microsoft.Band.Portable.Tiles.Pages;
using Microsoft.Band.Portable.Tiles.Pages.Data;
using Microsoft.Band.Portable.Sensors;

namespace SunBurn
{
	public class FrontPage : CarouselPage
	{
		private FrontPageManager _manager;
		private double _uvIndex;
		private Label _uvIndexLbl;

		public FrontPage ()
		{
			IsBusy = true;
			var positionService = DependencyService.Get<IPositionService> ();
			var dataService = new DataService ();
			var locationService = new LocationService (positionService, dataService);
			_manager = new FrontPageManager (locationService, dataService, new ExposureCalculator());
			Init ();
			IsBusy = false;
		}


		public FrontPage (ILocationService locationService, IDataService dataService){
			IsBusy = true;
			_manager = new FrontPageManager (locationService, dataService, new ExposureCalculator());
			Init ();
			IsBusy = false;
		}

		private async void Init(){
			var location = _manager.GetCurrentLocation ();
			try {
				var result = await _manager.GetResult (location);
				
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
				await this.DisplayAlert ("Error", ex.Message, "Ok");
			}
		}

		private async Task ConnectToMsBand(){
			var bandClientManager = BandClientManager.Instance;
			var devices = await bandClientManager.GetPairedBandsAsync ();
			var bandInfo = devices.FirstOrDefault ();
			if (bandInfo != null) {
				var bandClient = await bandClientManager.ConnectAsync (bandInfo);
				await SetupTilesForBand (bandClient);
			}
		}

		private async Task SetupTilesForBand(BandClient bandClient){
			var tileManager = bandClient.TileManager;
			var tileId = Guid.NewGuid ();
			short tId = 1;
			short buttonId = 2;
			short imageId = 3;
			var pageId = Guid.NewGuid();
			//var tiles = await tileManager.GetTilesAsync ();
			var availableTiles = await tileManager.GetRemainingTileCapacityAsync ();
			if (availableTiles > 0) {
				var tile = new BandTile (tileId) {
					//Icon = "",
					Name = "SunBurn",
					//SmallIcon = ""
				};

				var pageLayout = new PageLayout {
					Root = new ScrollFlowPanel {
						Rect = new PageRect(new PagePoint(0, 0), new PageSize(245, 105)),
						Orientation = FlowPanelOrientation.Vertical,
						Elements = {
							new TextBlock{
								ElementId = tId,
								Rect = new PageRect(new PagePoint(0, 0), new PageSize(229, 30)),
								ColorSource = ElementColorSource.BandBase,
								HorizontalAlignment = HorizontalAlignment.Left,
								VerticalAlignment = VerticalAlignment.Top
							},
							new TextButton {
								ElementId = buttonId,
								Rect = new PageRect(new PagePoint(0, 0), new PageSize(229, 43)),
								PressedColor = new BandColor(0, 127, 0)
							},
							new Microsoft.Band.Portable.Tiles.Pages.Icon {
								ElementId = imageId,
								Rect = new PageRect(new PagePoint(0, 0), new PageSize(229, 46)),
								Color = new BandColor(127, 127, 0),
								VerticalAlignment = VerticalAlignment.Center,
								HorizontalAlignment = HorizontalAlignment.Center
							}
						}
					}
				};

				tile.PageLayouts.Add (pageLayout);

				await tileManager.AddTileAsync (tile);

				var pageData = new PageData {
					PageId = pageId,
					PageLayoutIndex = 0,
					Data = {
						new TextBlockData {
							ElementId = tId,
							Text = "UvIndex"
						},
						new TextButtonData {
							ElementId = buttonId,
							Text = "StartTimer"
						},
						new ImageData {
							ElementId = imageId,
							ImageIndex = 0
						}
					}
				};
				// apply the data to the tile
				await tileManager.SetTilePageDataAsync(tileId, pageData);
			}
		}

		private async void ConnectToBandSensors(BandClient bandClient){
			var sensorManager = bandClient.SensorManager;
			var uvSensor = sensorManager.UltravioletLight;
			uvSensor.ReadingChanged += (o, args) => {
				var bandUvIndex = args.SensorReading.Level;
			};
			await uvSensor.StartReadingsAsync (BandSensorSampleRate.Ms128);
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