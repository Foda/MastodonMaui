using MastodonLib.Models;
using Refit;

namespace MastodonLib
{
    internal interface IMastodonApi
    {
        /// <summary>
        /// View statuses from followed users
        /// </summary>
        /// <param name="limit">Maximum number of results to return. Defaults to 20 statuses. Max 40 statuses</param>
        /// <param name="since_id">Return results newer than ID</param>
        /// <param name="max_id">Return results older than ID</param>
        /// <returns></returns>
        [Get("/api/v1/timelines/home")]
        Task<List<Status>> GetHomeTimeline(
            [Query] int limit = 20, [Query] string since_id = null, [Query] string max_id = null);

        /// <summary>
        /// View public statuses
        /// </summary>
        /// <param name="limit">Maximum number of results to return. Defaults to 20 statuses. Max 40 statuses</param>
        /// <param name="since_id">Return results newer than ID</param>
        /// <param name="max_id">Return results older than ID</param>
        /// <returns></returns>
        [Get("/api/v1/timelines/public")]
        Task<List<Status>> GetPublicTimeline(
            [Query] int limit = 20, [Query] string since_id = null, [Query] string max_id = null);

        [Get("/api/v1/accounts/verify_credentials")]
        Task<Account> GetCurrentUser();

        [Post("/api/v1/statuses/{id}/favourite")]
        Task FavoriteStatus(string id);

        [Post("/api/v1/statuses/{id}/unfavourite ")]
        Task UndoFavoriteStatus(string id);

        [Post("/api/v1/statuses/{id}/reblog")]
        Task ReblogStatus(string id);

        [Post("/api/v1/statuses/{id}/unreblog")]
        Task UndoReblogStatus(string id);
    }

    internal interface IMastodonAuthApi
    {
        [Post("/oauth/token")]
        Task<AuthToken> GetOAuthToken(
            string grant_type,
            string code,
            string client_id,
            string client_secret,
            string redirect_uri = "mastoauth://",
            string scope = "read write follow");
    }
}
