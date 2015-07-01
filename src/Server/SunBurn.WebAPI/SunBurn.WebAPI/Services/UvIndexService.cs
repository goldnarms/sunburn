using SunBurn.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunBurn.WebAPI.Services
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