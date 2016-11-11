using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DanceConventionClient.Services
{
	public static class HttpClientExtensions
	{
		public static async Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient client, string url, T model)
		{
			var json = JsonConvert.SerializeObject(model);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			return await client.PostAsync(url, content);
		}

		public static async Task<T> ReadAsAsync<T>(this HttpContent message)
		{
			var content = await message.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<T>(content);
		}
	}
}
