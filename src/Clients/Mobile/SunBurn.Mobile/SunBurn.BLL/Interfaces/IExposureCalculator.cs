using System;

namespace SunBurn
{
	public interface IExposureCalculator
	{
		TimeSpan CalculateTimeToSunburn(SkinType skinType, double uvIndex, double spfFactor, double altitude, bool inWater);
		double CalculateSpf(SkinType skinType, double uvIndex, double altitude, bool inWater, TimeSpan timeInSun);
	}
}

