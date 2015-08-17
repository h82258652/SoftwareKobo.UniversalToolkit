using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace SoftwareKobo.UniversalToolkit.Extensions
{
    /// <summary>
    /// HttpClient 扩展类。
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// 获取 json 并反序列化为实体。
        /// </summary>
        /// <typeparam name="T">实体类型。</typeparam>
        /// <param name="client">httpclient。</param>
        /// <param name="uri">请求的 Uri。</param>
        /// <returns>实体对象。</returns>
        public static async Task<T> GetJsonAsync<T>(this HttpClient client, Uri uri)
        {
            string json = await client.GetStringAsync(uri);
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(json));
        }
    }
}