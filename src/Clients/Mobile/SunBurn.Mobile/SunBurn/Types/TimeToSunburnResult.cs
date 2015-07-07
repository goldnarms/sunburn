using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunBurn
{
    public class TimeToSunburnResult
    {
        public string Location { get; set; }

		public Dictionary<string, SunburnResult> SunburnResults { get; set; }
    }

	public class SunburnResult
	{
		public double Celcius { get; set; }

		public double Fahrenheit { get; set; }

		public List<SpfTime> SpfTable { get; set; }

		public double UvIndex { get; set; }
	}
}
