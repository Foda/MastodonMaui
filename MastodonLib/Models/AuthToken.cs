using System.Text.Json.Serialization;

namespace MastodonLib.Models
{
    internal class AuthToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        public string Scope { get; set; }

        [JsonPropertyName("created_at")]
        public int CreatedAt { get; set; }
    }
}
