using SunBurn.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SunBurn.WebAPI.Controllers
{
    public class UvIndexController : ApiController
    {
        private IUvIndexService _uvIndexService;
        public UvIndexController()
        {
            _uvIndexService = new FakeServices.FakeUvIndexService();
        }

        public double Get(double lat, double lng)
        {
            return _uvIndexService.GetUvIndex(lat, lng, DateTime.Now);
        }
    }
}
