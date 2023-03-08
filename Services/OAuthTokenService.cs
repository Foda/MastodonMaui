using MastodonLib;
using MastodonLib.Auth;
using MastodonLib.Models;

namespace MastodonMaui.Services
{
    internal class OAuthTokenService : IAuthTokenStore
    {
        private readonly string _clientId = "6GJ0z5KyfVJ1iAbUKRk903035JEZEFfIb1mOsbhDrFg";
        private readonly string _clientSecret = "g9tGAmQ8mUQZpwzRZcZhWnxD4GgjCrXX_haGw6xoa_I";
        private readonly string _instanceUrl;
        private readonly MastodonAuthClient _authClient;

        private string _authCode = "";
        private string _authToken = "";

        private const string CALLBACK_URI = "mastoauth://";
        private const string STORAGE_AUTH_TOKEN_KEY = "oauth_token";
        private const string STORAGE_AUTH_TOKEN_CREATED = "oauth_token_created_at";

        public OAuthTokenService(string instanceUrl)
        {
            _instanceUrl = instanceUrl;
            _authClient = new MastodonAuthClient(instanceUrl);
        }

        public async Task<string> GetToken()
        {
            // Try and get the existing token. If we don't have one start the oauth login flow
            _authToken = await SecureStorage.Default.GetAsync(STORAGE_AUTH_TOKEN_KEY);

            if (string.IsNullOrEmpty(_authToken) && string.IsNullOrEmpty(_authCode))
            {
                await GetOAuthCode();
            }

            if (string.IsNullOrEmpty(_authToken))
            {
                AuthToken token = await _authClient.GetOAuthToken(_clientId, _clientSecret, _authCode);
                if (token != null)
                {
                    _authToken = token.AccessToken;
                    await SecureStorage.Default.SetAsync(STORAGE_AUTH_TOKEN_KEY, _authToken);
                    await SecureStorage.Default.SetAsync(STORAGE_AUTH_TOKEN_CREATED, token.CreatedAt.ToString());
                }
            }
            return _authToken;
        }

        private async Task GetOAuthCode()
        {
            string auth = $"{_instanceUrl}/oauth/authorize" +
                $"?client_id={_clientId}" +
                $"&scope=read+write+follow" +
                $"&redirect_uri={CALLBACK_URI}" +
                $"&response_type=code";

#if WINDOWS
            // https://github.com/dotnet/maui/issues/2702
            var result = await WinUIEx.WebAuthenticator.AuthenticateAsync(
                new Uri(auth), new Uri(CALLBACK_URI));
#else
            var result = await Microsoft.Maui.Authentication.WebAuthenticator.AuthenticateAsync(new WebAuthenticatorOptions()
            {
                Url = new Uri(auth),
                CallbackUrl = new Uri(CALLBACK_URI)
            });
#endif
            _authCode = result.Properties["code"];
        }
    }
}
