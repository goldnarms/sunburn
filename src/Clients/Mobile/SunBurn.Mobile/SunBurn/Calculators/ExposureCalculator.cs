using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunBurn.Calculators
{
	public class ExposureCalculator: IExposureCalculator
    {
		private Dictionary<SkinType, double> _factorTable = new Dictionary<SkinType, double>
		{
			{ SkinType.VeryLight, 67 },
			{ SkinType.Light, 100 },
			{ SkinType.Medium, 200 },
			{ SkinType.Tan, 300 },
			{ SkinType.Dark, 400 },
			{ SkinType.Black, 500 }
		};

        public ExposureCalculator()
        {

        }

        public TimeSpan CalculateTimeToSunburn(SkinType skinType, double uvIndex, double spfFactor, double altitude, bool inWater)
        {
            var uvWithAlt = uvIndex * (1 + (altitude * 0.0016)) * (inWater ? 1.5 : 1);
            
            return TimeSpan.FromMinutes((_factorTable[skinType] / uvWithAlt) * spfFactor);
        }

        public double CalculateSpf(SkinType skinType, double uvIndex, double altitude, bool inWater, TimeSpan timeInSun)
        {
            var uvWithAlt = uvIndex * (1 + (altitude * 0.0016)) * (inWater ? 1.5 : 1);
            return (timeInSun.TotalMinutes / (_factorTable[skinType] / uvWithAlt));
        }
    }
}
