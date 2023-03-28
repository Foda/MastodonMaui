using MastodonLib;
using MastodonLib.Auth;

namespace MastodonMaui.Services
{
    internal interface ISiteInstance
    {
        IAuthTokenStore TokenStore { get; }
        IMastodonClient Client { get; }
        string InstanceUrl { get; }
    }
}
