using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MastodonLib.Models
{
    public class Status : ApiObject
    {

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("in_reply_to_id")]
        public string InReplyToId { get; set; }

        [JsonPropertyName("in_reply_to_account_id")]
        public string InReplyToAccountId { get; set; }

        /// <summary>
        /// Optional: Have you boosted this status?
        /// </summary>
        public bool Reblogged { get; set; }

        /// <summary>
        /// Optional: Have you favourited this status?
        /// </summary>
        public bool Favourited { get; set; }

        [JsonPropertyName("reblogs_count")]
        public int ReblogsCount { get; set; }

        [JsonPropertyName("replies_count")]
        public int RepliesCount { get; set; }

        [JsonPropertyName("favourites_count")]
        public int FavouritesCount { get; set; }

        /// <summary>
        /// HTML-encoded status content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Plain-text source of a status. Returned instead of content when status is deleted,
        /// so the user may redraft from the source text without the client having to 
        /// reverse-engineer the original text from the HTML content.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// public, unlisted, private, direct
        /// </summary>
        public string Visibility { get; set; }

        /// <summary>
        /// Subject or summary line, below which status content is collapsed until expanded.
        /// </summary>
        public string SpoilerText { get; set; }

        public string Uri { get; set; }
        public Account Account { get; set; }
        public Status Reblog { get; set; }
        public List<Mention> Mentions { get; set; }
        public List<Tag> Tags { get; set; }

        [JsonPropertyName("media_attachments")]
        public List<MediaAttachment> MediaAttachments { get; set; }

        public Card Card { get; set; }
    }
}
