using MastodonLib;

namespace MastodonMaui.Services
{
    internal class SiteInstanceService : ISiteInstance
    {
        private readonly OAuthTokenService _authService;

        public MastodonClient Client { get; }
        public string InstanceUrl { get; }

        public SiteInstanceService(string instanceUrl)
        {
            _authService = new OAuthTokenService(instanceUrl);
            Client = new MastodonClient(_authService, instanceUrl);
            InstanceUrl = instanceUrl;
        }
    }
}
