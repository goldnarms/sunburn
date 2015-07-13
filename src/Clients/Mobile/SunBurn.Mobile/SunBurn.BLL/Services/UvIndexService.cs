using System;
using System.Collections.Generic;
using System.Linq;

namespace SunBurn
{
    public class UvIndexService : IUvIndexService
    {
        public UvIndexService()
        {

        }

        public double GetUvIndex(double lat, double lng, DateTime time)
        {
            var rnd = new Random();
            return rnd.Next(0, 5);
        }
    }
}