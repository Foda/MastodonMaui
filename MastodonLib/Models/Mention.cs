namespace MastodonLib.Models
{
    internal class Mention : ApiObject
    {
        public string UserName { get; set; }
        public string Url { get; set; }
    }
}
