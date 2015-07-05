using ModernHttpClient;
using Newtonsoft.Json;
using SunBurn.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Serialization;

namespace SunBurn.WebAPI.Controllers
{
    public class UvIndexController : ApiController
    {
        private string EndpointUrl = "http://api.worldweatheronline.com/premium/v1/weather.ashx?key={0}&q={1},{2}&num_of_days=3&tp=3&format=json";
        public UvIndexController()
        {

        }

        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient(new NativeMessageHandler());
            client.DefaultRequestHeaders.Add("Accept", "application/xml");
            return client;
        }

        public async Task<SunBurn.BLL.Types.Response> GetData(Tuple<double, double, double> position, double spf, BLL.Types.SkinType skinType)
        {
            var key = "e748e42e457095f423044e0f7e693";
            var client = GetClient();
            var result = await client.GetStringAsync(string.Format(EndpointUrl, key, position.Item1, position.Item2));
            return JsonConvert.DeserializeObject<SunBurn.BLL.Types.Response>(result);
        }

        public double Get(double lat, double lng)
        {
            return _uvIndexService.GetUvIndex(lat, lng, DateTime.Now);
        }
    }
}
