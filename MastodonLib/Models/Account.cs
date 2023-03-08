using System.Text.Json.Serialization;

namespace MastodonLib.Models
{
    internal class Account
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

        [JsonPropertyName("avatar")]
        public string AvatarUrl { get; set; }
    }
}
