using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunBurn.BLL.Types
{
    public class Response
    {
        public string Location { get; set; }

        public DateTime Time { get; set; }

        public double Celcius { get; set; }

        public double Fahrenheit { get; set; }

        public List<SpfTime> SpfTable { get; set; }

        public double UvIndex { get; set; }
    }
}
