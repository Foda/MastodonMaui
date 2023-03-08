using MastodonLib.Auth;
using MastodonLib.Models;
using Refit;

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
