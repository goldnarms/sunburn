using System;
using System.Threading.Tasks;

namespace SunBurn
{
	public interface IDataService
	{
		Task<WeatherResponse> GetWeatherData (Tuple<double, double> position);
		Task<LocationResponse> GetLocationName (Tuple<double, double> position);
	}
}

