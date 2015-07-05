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

		public TimeToSunburnResult GetResult(Tuple<double, double, double> position){
			var result = new TimeToSunburnResult {
				SunburnResults = new Dictionary<DateTime, SunburnResult>()
			};
			var currentTime = DateTime.Now;
			for (int i = 0; i < 3; i++) {
				var weatherData = _dataService.GetWeatherData(new Tuple<double, double>(position.Item1, position.Item2), currentTime.AddDays(i));
				if (i == 0) {
					result.Location = weatherData.LocationName;
				}
				var sunburnResult = new SunburnResult();
				sunburnResult.Celcius = weatherData.Celsius;
				sunburnResult.Fahrenheit = weatherData.Fahrenheit;
				sunburnResult.UvIndex = weatherData.UvIndex;
				sunburnResult.SpfTable = new List<SpfTime> ();
				foreach (var spf in _sunProtectionFactors) {
					var timeToSunburnResult = _exposureCalculator.CalculateTimeToSunburn (Settings.SkinTypeSetting, weatherData.UvIndex, spf, position.Item3, false);
					sunburnResult.SpfTable.Add(new SpfTime{Spf = spf, Time = timeToSunburnResult});
				}
				result.SunburnResults = new Dictionary<DateTime, SunburnResult> {
					{ currentTime.AddDays(i), sunburnResult}
				};
			}
			return result; 
		}
	}
}

