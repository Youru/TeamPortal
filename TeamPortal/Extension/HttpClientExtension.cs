using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace TeamPortal.Extension
{
    public static class HttpClientExtension
    {
        public static async Task<T> GetObjectAsync<T>(this HttpClient client, string url)
        {
            var response = await client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<T>(response);
        }
    }
}
