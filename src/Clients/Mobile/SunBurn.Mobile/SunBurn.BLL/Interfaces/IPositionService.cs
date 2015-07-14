using System;

namespace SunBurn
{
	public interface IPositionService
	{
		Tuple<double, double, double> GetCurrentPosition();
	}
}