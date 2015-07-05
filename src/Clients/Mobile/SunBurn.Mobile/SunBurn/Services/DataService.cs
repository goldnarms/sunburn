using System;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ModernHttpClient;
using Newtonsoft.Json;

namespace SunBurn
{
	public class DataService : IDataService
	{
		private const string Url = "";
		public DataService ()
		{
		}
			
		private HttpClient GetClient(){
			HttpClient client = new HttpClient (new NativeMessageHandler ());
			client.DefaultRequestHeaders.Add ("Accept", "application/json");
			return client;
		}

		public WeatherData GetWeatherData(Tuple<double, double> position, DateTime time){
			var weatherData = new WeatherData {
				Celsius = 33,
				Fahrenheit = 58,
				LocationName = "Bangkok",
				UvIndex = 12
			};
			return weatherData;
//			var client = GetClient ();
//			var result = await client.GetStringAsync (Url);
//			
//			return JsonConvert.DeserializeObject<Response>(result);
		}
	}
}