using MastodonLib.Models;

namespace MastodonLib
{
    internal interface IMastodonClient
    {
        Task<List<Status>> GetHomeTimeline(int limit = 20, string sinceId = null, string maxId = null);
        Task<List<Status>> GetPublicTimeline(int limit = 20, string sinceId = null, string maxId = null);
        Task<StatusContext> GetStatusContext(string status_id);
        Task<Status> PostStatus(string status, string visibility = "public", string replyToStatusId = null);
        Task<ScheduledStatus> PostScheduledStatus(string status, DateTime scheduledAt, string visibility = "public", string replyToStatusId = null);
        Task<Account> GetCurrentUser();
        Task ToggleFavoriteStatus(string id, bool isFavorited);
        Task ToggleReblogStatus(string id, bool shouldReblog);
        Task<List<Tag>> GetTrendingTags(int limit = 5, int offset = 0);
    }
}
