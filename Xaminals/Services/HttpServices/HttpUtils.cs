using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Xaminals.Services.HttpServices
{
    public static class HttpUtils
    {
        public static async Task<T> ToGenericObject<T>(this HttpResponseMessage response) 
        {
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}
