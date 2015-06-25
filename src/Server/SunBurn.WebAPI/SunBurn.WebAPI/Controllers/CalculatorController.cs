using SunBurn.BLL.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SunBurn.WebAPI.Controllers
{
    public class CalculatorController : ApiController
    {
        private ExposureCalculator _exposureCalculator;
        public CalculatorController()
        {
            _exposureCalculator = new ExposureCalculator();
        }
        public TimeSpan Get()
        {
            return _exposureCalculator.CalculateTimeToSunburn(BLL.Types.SkinType.Medium, 20, 15, 200, false);
        }
    }
}
