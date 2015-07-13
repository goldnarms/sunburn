using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunBurn.Managers
{
	public class FrontPageManager
	{
		private ILocationService _locationService;
		private IDataService _dataService;
		private IExposureCalculator _exposureCalculator;
		private List<int> _sunProtectionFactors = new List<int>{0, 15, 30, 50};
		public FrontPageManager (ILocationService locationService, IDataService dataService, IExposureCalculator exposureCalculator)
		{
			_locationService = locationService;
			_dataService = dataService;
			_exposureCalculator = exposureCalculator;
		}

		public UserLocation GetCurrentLocation(){
			return new UserLocation {
				Position = Settings.Position,
				Name = Settings.LocationName
			};
		}

		public async Task<TimeToSunburnResult> GetResult(UserLocation userLocation){
			var result = new TimeToSunburnResult {
				SunburnResults = new Dictionary<string, SunburnResult>()
			};
			var locationData = await _dataService.GetLocationName (new Tuple<double, double> (userLocation.Position.Latitude, userLocation.Position.Longitude));
			var weatherData = await _dataService.GetWeatherData(new Tuple<double, double>(userLocation.Position.Latitude, userLocation.Position.Longitude));
			result.Location = locationData.results.Count() > 0 ? locationData.results.First ().address_components.Where (a => a.types.Contains ("administrative_area_level_2") || a.types.Contains ("administrative_area_level_1")).First ().long_name : "";
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
				result.SunburnResults.Add (weatherResult.date, sunburnResult);
			}
			return result; 
		}
	}
}

