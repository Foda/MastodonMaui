using MastodonLib;
using MastodonLib.Auth;

namespace MastodonMaui.Services
{
    internal class SiteInstanceService : ISiteInstance
    {
        public IAuthTokenStore TokenStore { get; }
        public IMastodonClient Client { get; }
        public string InstanceUrl { get; }

        public SiteInstanceService(string instanceUrl)
        {
            TokenStore = new OAuthTokenService(instanceUrl);
            Client = new MastodonClient(TokenStore, instanceUrl);
            InstanceUrl = instanceUrl;
        }
    }
}
