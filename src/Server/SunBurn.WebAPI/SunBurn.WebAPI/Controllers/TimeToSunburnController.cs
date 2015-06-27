using SunBurn.BLL.Calculators;
using SunBurn.Interfaces;
using System;
using System.Web.Http;

namespace SunBurn.WebAPI.Controllers
{
    public class TimeToSunburnController : ApiController
    {
        private ExposureCalculator _exposureCalculator;
        private IUvIndexService _uvIndexService;
        public TimeToSunburnController()
        {
            //http://dataapi.wxc.com/faq.html
            _exposureCalculator = new ExposureCalculator();
            _uvIndexService = new FakeServices.FakeUvIndexService();
        }
        public TimeSpan Get(double lat, double lng, double altitude, double spfFactor, BLL.Types.SkinType skinType)
        {
            var uvIndex = _uvIndexService.GetUvIndex(lat, lng, DateTime.Now);
            return _exposureCalculator.CalculateTimeToSunburn(skinType, uvIndex, spfFactor, altitude, false);
        }
    }
}
