using SunBurn.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunBurn.FakeServices
{
    public class FakeUvIndexService : IUvIndexService
    {
        public FakeUvIndexService()
        {

        }

        public double GetUvIndex(double lat, double lng, DateTime time)
        {
            var rnd = new Random();
            return rnd.Next(0, 5);
        }
    }
}
