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
		private string EndpointUrl = "http://api.worldweatheronline.com/free/v2/weather.ashx?key={0}&q={1},{2}&num_of_days=3&tp=3&format=json";
		private const string Key = "e748e42e457095f423044e0f7e693";
		private const string GeoCodingKey = "AIzaSyDzsUSC9losX41le-_lE6cEt9VgPgydFBQ";
		private string GeoCodingUrl = "https://maps.googleapis.com/maps/api/geocode/json?latlng={0},{1}&key={2}";
		public DataService ()
		{
		}
			
		private HttpClient GetClient(){
			HttpClient client = new HttpClient (new NativeMessageHandler ());
			client.DefaultRequestHeaders.Add ("Accept", "application/json");
			return client;
		}

		public async Task<WeatherResponse> GetWeatherData(Tuple<double, double> position){
			var client = GetClient ();
			var result = await client.GetStringAsync (string.Format(EndpointUrl, Key, position.Item1, position.Item2));
			
			return JsonConvert.DeserializeObject<WeatherResponse>(result);
		}

		public async Task<LocationResponse> GetLocationName(Tuple<double, double> position){
			var client = GetClient ();
			var result = await client.GetStringAsync (string.Format(GeoCodingUrl, position.Item1, position.Item2, GeoCodingKey));

			return JsonConvert.DeserializeObject<LocationResponse>(result);
		}

		public async Task<LocationResponse> GetPositionForLocation(string location){
			var client = GetClient ();
			var result = await client.GetStringAsync (string.Format(GeoCodingUrl, location, GeoCodingKey));

			return JsonConvert.DeserializeObject<LocationResponse>(result);

		}
	}
}