using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SunBurn.Managers
{
	public class FrontPageManager
	{
		private ILocationManager _locationMngr;
		private IDataService _dataService;
		private IExposureCalculator _exposureCalculator;
		private List<int> _sunProtectionFactors = new List<int>{0, 15, 30, 50};
		public FrontPageManager (ILocationManager locationMngr, IDataService dataService, IExposureCalculator exposureCalculator)
		{
			_locationMngr = locationMngr;
			_dataService = dataService;
			_exposureCalculator = exposureCalculator;
		}

		public Tuple<double, double, double> GetCurrentLocation(){
			return _locationMngr.GetLocation();
		}

		public async Task<TimeToSunburnResult> GetResult(Tuple<double, double, double> position){
			var result = new TimeToSunburnResult {
				SunburnResults = new Dictionary<string, SunburnResult>()
			};
			try {
				var locationData = await _dataService.GetLocationName (new Tuple<double, double> (position.Item1, position.Item2));
				result.Location = locationData.results.Count() > 0 ? locationData.results.First ().address_components.Where (a => a.types.Contains ("administrative_area_level_2") || a.types.Contains ("administrative_area_level_1")).First ().long_name : "";
				
			} catch (Exception ex) {
				throw ex;
			}
			try {
				var weatherData = await _dataService.GetWeatherData(new Tuple<double, double>(position.Item1, position.Item2));
				
				foreach (var weatherResult in weatherData.data.weather) {
					var hourIndex = DateTime.UtcNow.Hour -1 > weatherResult.hourly.Count() -1 ? weatherResult.hourly.Count() - 1 : DateTime.UtcNow.Hour -1;
					var sunburnResult = new SunburnResult();

					sunburnResult.Celcius = weatherResult.hourly[hourIndex].tempC;
					sunburnResult.Fahrenheit = weatherResult.hourly[hourIndex].tempF;
					sunburnResult.UvIndex = weatherResult.uvIndex;
					sunburnResult.SpfTable = new List<SpfTime> ();
					foreach (var spf in _sunProtectionFactors) {
						var timeToSunburnResult = _exposureCalculator.CalculateTimeToSunburn (Settings.SkinTypeSetting, weatherResult.uvIndex, spf, position.Item3, false);
						sunburnResult.SpfTable.Add(new SpfTime{Spf = spf, Time = timeToSunburnResult.ToString("c")});
					}
					result.SunburnResults.Add (weatherResult.date, sunburnResult);
				}
			} catch (Exception ex) {
				throw ex;			
			}
			return result; 
		}
	}
}

