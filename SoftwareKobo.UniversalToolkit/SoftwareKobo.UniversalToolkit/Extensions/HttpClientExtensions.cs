using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace SoftwareKobo.UniversalToolkit.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T> GetJsonAsync<T>(this HttpClient client, Uri uri)
        {
            string json = await client.GetStringAsync(uri);
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(json));
        }
    }
}