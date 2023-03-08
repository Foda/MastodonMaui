using System.Net.Http.Headers;

namespace MastodonLib.Auth
{
    internal class AuthHeaderHandler : DelegatingHandler
    {
        private readonly IAuthTokenStore _authTokenStore;

        public AuthHeaderHandler(IAuthTokenStore authTokenStore)
        {
            _authTokenStore = authTokenStore ?? throw new ArgumentNullException(nameof(authTokenStore));
            InnerHandler = new HttpClientHandler();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _authTokenStore.GetToken();

            //potentially refresh token here if it has expired etc

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
