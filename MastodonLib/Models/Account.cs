using System.Text.Json.Serialization;

namespace MastodonLib.Models
{
    public class Account : ApiObject
    {
        public string UserName { get; set; }

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

        [JsonPropertyName("avatar")]
        public string AvatarUrl { get; set; }
    }
}
