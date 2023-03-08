using MastodonLib.Models;
using Refit;

namespace MastodonLib
{
    internal class MastodonAuthClient
    {
        private readonly IMastodonAuthApi _api;

        public MastodonAuthClient(string instanceUrl)
        {
            _api = RestService.For<IMastodonAuthApi>(
                new HttpClient()
                {
                    BaseAddress = new Uri(instanceUrl)
                }
            );
        }

        public async Task<AuthToken> GetOAuthToken(string client_id, string client_secret, string code)
        {
            return await _api.GetOAuthToken("authorization_code", code, client_id, client_secret);
        }
    }
}
