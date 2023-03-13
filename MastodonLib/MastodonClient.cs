using MastodonLib.Auth;
using MastodonLib.Models;
using Refit;
using System.Collections.Generic;

namespace MastodonLib
{
    internal class MastodonClient
    {
        private readonly IMastodonApi _api;

        public MastodonClient(IAuthTokenStore authTokenStore, string instanceUrl)
        {
            _api = RestService.For<IMastodonApi>(
                new HttpClient(new AuthHeaderHandler(authTokenStore))
                {
                    BaseAddress = new Uri(instanceUrl)
                }
            );
        }

        public async Task<List<Status>> GetHomeTimeline(
            int limit = 20, string sinceId = null, string maxId = null)
        {
            return await _api.GetHomeTimeline(limit, sinceId, maxId);
        }

        public async Task<List<Status>> GetPublicTimeline(
            int limit = 20, string sinceId = null, string maxId = null)
        {
            return await _api.GetPublicTimeline(limit, sinceId, maxId);
        }

        public async Task<Status> PostStatus(string status, string replyToStatusId = null)
        {
            var theBody = new Dictionary<string, object> {
                { "status", status }
            };

            if (!string.IsNullOrEmpty(replyToStatusId))
            {
                theBody.Add("in_reply_to_id", replyToStatusId);
            }

            return await _api.PostStatus(theBody);
        }

        public async Task<ScheduledStatus> PostScheduledStatus(string status, DateTime scheduledAt, string replyToStatusId = null)
        {
            var theBody = new Dictionary<string, object> {
                { "status", status },
                { "scheduled_at", scheduledAt.ToString("o") }
            };

            if (!string.IsNullOrEmpty(replyToStatusId))
            {
                theBody.Add("in_reply_to_id", replyToStatusId);
            }
            return await _api.PostScheduledStatus(theBody);
        }

        public async Task<Account> GetCurrentUser()
        {
            return await _api.GetCurrentUser();
        }

        public async Task ToggleFavoriteStatus(string id, bool isFavorited)
        {
            if (isFavorited)
            {
                await _api.FavoriteStatus(id);
            }
            else
            {
                await _api.UndoFavoriteStatus(id);
            }
        }

        public async Task ToggleReblogStatus(string id, bool shouldReblog)
        {
            if (shouldReblog)
            {
                await _api.ReblogStatus(id);
            }
            else
            {
                await _api.UndoReblogStatus(id);
            }
        }
    }
}
