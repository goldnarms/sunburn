﻿using System;
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
			
		private async Task<HttpClient> GetClient(){
			HttpClient client = new HttpClient (new NativeMessageHandler ());
			client.DefaultRequestHeaders.Add ("Accept", "application/json");
			return client;
		}

		public async Task<SunBurn.BLL.Types.Response> GetData(Tuple<double, double, double> position, double spf, BLL.Types.SkinType skinType){
			var client = await GetClient ();
			var result = await client.GetStringAsync (Url);
			return JsonConvert.DeserializeObject<SunBurn.BLL.Types.Response>(result);
		}
	}
}