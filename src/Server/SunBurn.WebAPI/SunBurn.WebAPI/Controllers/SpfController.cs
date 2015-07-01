using SunBurn.BLL.Calculators;
using SunBurn.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SunBurn.WebAPI.Controllers
{
    public class SpfController : ApiController
    {
        private ExposureCalculator _exposureCalculator;
        private IUvIndexService _uvIndexService;
        public SpfController()
        {
            _exposureCalculator = new ExposureCalculator();
            _uvIndexService = new FakeServices.FakeUvIndexService();
        }

        public double Get(double lat, double lng, double altitude, TimeSpan timeInSun, BLL.Types.SkinType skinType)
        {
            var uvIndex = _uvIndexService.GetUvIndex(lat, lng, DateTime.Now);
            return _exposureCalculator.CalculateSpf(skinType, uvIndex, altitude, false, timeInSun);
        }
    }
}
