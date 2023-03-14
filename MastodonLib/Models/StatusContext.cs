using System.Text.Json.Serialization;

namespace MastodonLib.Models
{
    public class StatusContext
    {
        [JsonPropertyName("ancestors")]
        public List<Status> Ancestors { get; set; }
        [JsonPropertyName("descendants")]
        public List<Status> Descendants { get; set; }
    }
}
