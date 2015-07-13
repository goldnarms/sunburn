using System;

namespace SunBurn
{
	public interface ILocationManager
	{
		Tuple<double, double, double> GetLocation();
	}
}

