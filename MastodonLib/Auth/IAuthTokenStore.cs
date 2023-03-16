using MastodonLib.Models;

namespace MastodonLib.Auth
{
    internal interface IAuthTokenStore
    {
        void ClearToken();
        Task<string> GetToken();
    }
}
