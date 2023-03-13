using System.Text.Json.Serialization;

namespace MastodonLib.Models
{
    public class MediaAttachment
    {
        public string Id { get; set; }
        public string Url { get; set; }

        [JsonPropertyName("preview_url")]
        public string PreviewUrl { get; set; }

        /// <summary>
        /// One of: image, video, gifv, audio
        /// </summary>
        public string Type { get; set; }
    }
}
