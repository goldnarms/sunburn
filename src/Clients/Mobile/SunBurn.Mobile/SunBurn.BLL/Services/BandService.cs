using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Band.Portable;
using Microsoft.Band.Portable.Tiles;
using Microsoft.Band.Portable.Tiles.Pages;
using Microsoft.Band.Portable.Tiles.Pages.Data;
using Microsoft.Band.Portable.Sensors;

namespace SunBurn.BLL
{
	public class BandService : IBandService
	{
		public bool HasDevice { get; private set; }
		private BandClient _bandClient;
		private BandClientManager _bandClientManager;
		private BandTileManager _bandTileManager;
		private Guid _tileId;
		private Guid _pageId;
		private short _uvIndexId;
		private short _timeToBurnId;
		private short _btnStartTimerId;


		public BandService ()
		{
			_tileId = Guid.NewGuid();
			_pageId = Guid.NewGuid ();
			_bandClientManager = BandClientManager.Instance;
			_uvIndexId = 0;
			_timeToBurnId = 1;
			_btnStartTimerId = 2;
		}

		public async Task SetupDevice ()
		{
			var devices = await _bandClientManager.GetPairedBandsAsync ();
			var bandInfo = devices.FirstOrDefault ();
			if (bandInfo != null) {
				HasDevice = true;
				_bandClient = await _bandClientManager.ConnectAsync (bandInfo);
				_bandTileManager = _bandClient.TileManager;
				await SetupTilesForBand ();
			} else {
				HasDevice = false;
			}
		}

		public async Task UpdateTile (SunburnResult result)
		{
				var pageData = new PageData {
					PageId = _pageId,
					PageLayoutIndex = 0,
					Data = {
						new TextBlockData {
							ElementId = _uvIndexId,
							Text = string.Format ("UV Index: {0}", result.UvIndex.ToString ())
						},
						new TextButtonData {
							ElementId = _btnStartTimerId,
							Text = "Start timer"
						},
						new TextBlockData {
							ElementId = _timeToBurnId,
							Text = string.Format ("Time to burn: {0}", TimeSpan.FromHours (1).ToString ("c"))
						}
					}
				};
				// apply the data to the tile
				await _bandTileManager.SetTilePageDataAsync (_tileId, pageData);
		}

		private async Task SetupTilesForBand(){
			//var tiles = await tileManager.GetTilesAsync ();
			var availableTiles = await _bandTileManager.GetRemainingTileCapacityAsync ();
			if (availableTiles > 0) {
				var tile = new BandTile (_tileId) {
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
								ElementId = _uvIndexId,
								Rect = new PageRect(new PagePoint(0, 0), new PageSize(229, 30)),
								ColorSource = ElementColorSource.BandBase,
								HorizontalAlignment = HorizontalAlignment.Left,
								VerticalAlignment = VerticalAlignment.Top
							},
							new TextBlock{
								ElementId = _timeToBurnId,
								Rect = new PageRect(new PagePoint(0, 0), new PageSize(229, 36)),
								ColorSource = ElementColorSource.BandBase,
								HorizontalAlignment = HorizontalAlignment.Left,
								VerticalAlignment = VerticalAlignment.Top
							},
							new TextButton {
								ElementId = _btnStartTimerId,
								Rect = new PageRect(new PagePoint(0, 0), new PageSize(229, 43)),
								PressedColor = new BandColor(0, 127, 0)
							}
						}
					}
				};

				tile.PageLayouts.Add (pageLayout);

				await _bandTileManager.AddTileAsync (tile);
			}
		}

		private async void ConnectToBandSensors(){
			var sensorManager = _bandClient.SensorManager;
			var uvSensor = sensorManager.UltravioletLight;
			uvSensor.ReadingChanged += (o, args) => {
				var bandUvIndex = args.SensorReading.Level;
			};
			await uvSensor.StartReadingsAsync (BandSensorSampleRate.Ms128);
		}

		public async Task Notify (string title, string message)
		{
			if (HasDevice) {
				var notifictionManager = _bandClient.NotificationManager;
				// send a notification to the Band with a dialog as well as a page
				await notifictionManager.SendMessageAsync(
					_tileId, 
					title, 
					message, 
					DateTime.Now, 
					true);
			}
		}
	}
}