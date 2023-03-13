using System.Text.Json.Serialization;

namespace MastodonLib.Models
{
    internal class ScheduledStatus : ApiObject
    {
        [JsonPropertyName("scheduled_at")]
        public DateTime ScheduledAt { get; set; }

        public ScheduledStatusParams Params { get; set; }
    }

    internal class ScheduledStatusParams
    {
        public string Text { get; set; }

        [JsonPropertyName("in_reply_to_id")]
        public string InReplyToId { get; set; }

        [JsonPropertyName("application_id")]
        public int ApplicationId { get; set; }
    }
}
