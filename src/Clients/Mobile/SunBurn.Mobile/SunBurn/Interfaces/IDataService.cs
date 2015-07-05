using System;
using System.Threading.Tasks;

namespace SunBurn
{
	public interface IDataService
	{
		WeatherData GetWeatherData (Tuple<double, double> position, DateTime time);
	}
}

