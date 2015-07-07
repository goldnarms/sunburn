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

		public async TimeToSunburnResult GetResult(Tuple<double, double, double> position){
			var result = new TimeToSunburnResult {
				SunburnResults = new Dictionary<DateTime, SunburnResult>()
			};
			var locationData = await _dataService.GetLocationName (new Tuple<double, double> (position.Item1, position.Item2));
			var weatherData = await _dataService.GetWeatherData(new Tuple<double, double>(position.Item1, position.Item2));
			result.Location = locationData.results.Count() > 0 ? locationData.results.First ().address_components.Where (a => a.types.Contains ("administrative_area_level_2") || a.types.Contains ("administrative_area_level_2")).First ().short_name : "";
			foreach (var weatherResult in weatherData.data.weather) {
				var hourIndex = DateTime.Now.Hour;
				var sunburnResult = new SunburnResult();

				sunburnResult.Celcius = weatherResult.hourly[hourIndex].tempC;
				sunburnResult.Fahrenheit = weatherResult.hourly[hourIndex].tempF;
				sunburnResult.UvIndex = weatherResult.uvIndex;
				sunburnResult.SpfTable = new List<SpfTime> ();
				foreach (var spf in _sunProtectionFactors) {
					var timeToSunburnResult = _exposureCalculator.CalculateTimeToSunburn (Settings.SkinTypeSetting, weatherResult.uvIndex, spf, position.Item3, false);
					sunburnResult.SpfTable.Add(new SpfTime{Spf = spf, Time = timeToSunburnResult});
				}
				result.SunburnResults.Add (weatherResult.date, sunburnResult);
			}
			return result; 
		}
	}
}

