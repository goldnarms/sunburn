using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunBurn.Managers
{
	public class HomePageManager
	{
		private ILocationService _locationService;
		private IDataService _dataService;
		private IExposureCalculator _exposureCalculator;
		private List<int> _sunProtectionFactors = new List<int>{0, 15, 30, 50};
		public HomePageManager (ILocationService locationService, IDataService dataService, IExposureCalculator exposureCalculator)
		{
			_locationService = locationService;
			_dataService = dataService;
			_exposureCalculator = exposureCalculator;
		}

		public BLL.UserLocation GetCurrentLocation(){
			return new BLL.UserLocation {
				Position = Settings.Position,
				Name = Settings.LocationName
			};
		}

		public async Task<TimeToSunburnResult> GetResult(BLL.UserLocation userLocation){
			var result = new TimeToSunburnResult {
				SunburnResults = new Dictionary<string, SunburnResult>()
			};
			try {
				var locationData = await _dataService.GetLocationName (new Tuple<double, double> (userLocation.Position.Latitude, userLocation.Position.Longitude));
				result.Location = locationData.results.Count() > 0 ? FormatterHelper.FormatAddress(locationData.results.First ()) : "";
				
			} catch (Exception ex) {
				throw ex;
			}
			try {
				var weatherData = await _dataService.GetWeatherData(new Tuple<double, double>(userLocation.Position.Latitude, userLocation.Position.Longitude));
				
				foreach (var weatherResult in weatherData.data.weather) {
					var hourIndex = DateTime.UtcNow.Hour -1 > weatherResult.hourly.Count() -1 ? weatherResult.hourly.Count() - 1 : DateTime.UtcNow.Hour -1;
					var sunburnResult = new SunburnResult();

				sunburnResult.Celcius = weatherResult.hourly[hourIndex].tempC;
				sunburnResult.Fahrenheit = weatherResult.hourly[hourIndex].tempF;
				sunburnResult.UvIndex = weatherResult.uvIndex;
				sunburnResult.SpfTable = new List<SpfTime> ();
				foreach (var spf in _sunProtectionFactors) {
					var timeToSunburnResult = _exposureCalculator.CalculateTimeToSunburn (Settings.SkinTypeSetting, weatherResult.uvIndex, spf, userLocation.Position.Altitude, false);
					sunburnResult.SpfTable.Add(new SpfTime{Spf = spf, Time = timeToSunburnResult.ToString("c")});
					}
				}
			} catch (Exception ex) {
				throw ex;			
			}
			return result; 
		
	}
		}

}


