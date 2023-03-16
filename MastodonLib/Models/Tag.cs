namespace MastodonLib.Models
{
    public class Tag
    {
        /// <summary>
        /// The value of the hashtag after the # sign
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A link to the hashtag on the instance
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Usage statistics for given days (typically the past week)
        /// </summary>
        public List<TagHistory> History { get; set; }

        public string HistoryDescription
        {
            get
            {
                if (History != null && History.Count > 0)
                    return $"{History[0].Uses} toots";
                return "0 toots";
            }
        }
    }

    public class TagHistory
    {
        /// <summary>
        /// UNIX timestamp on midnight of the given day
        /// </summary>
        public string Day { get; set; }

        /// <summary>
        /// The counted usage of the tag within that day
        /// </summary>
        public string Uses { get; set; }

        /// <summary>
        /// The total of accounts using the tag within that day
        /// </summary>
        public string Accounts { get; set; }
    }
}
