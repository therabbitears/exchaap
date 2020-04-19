using Newtonsoft.Json;
using System;
namespace Xaminals.Models
{
    public class TokenModel
    {
        [JsonProperty("userName")]
        public string vendor { get; set; }
        [JsonProperty("access_token")]
        public string token { get; set; }
        [JsonProperty(".expires")]
        public DateTime expiration { get; set; }
    }

    public class ImageModel
    {
        public string url { get; set; }
        public bool uploaded { get; set; }
    }
}
