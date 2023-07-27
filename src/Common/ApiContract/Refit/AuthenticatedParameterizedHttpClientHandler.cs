namespace ApiContract.Refit;

public class AuthenticatedParameterizedHttpClientHandler : DelegatingHandler
{
    private readonly IAuthTokenService authTokenService;

    public AuthenticatedParameterizedHttpClientHandler(
            IAuthTokenService authTokenService,
            HttpMessageHandler? innerHandler = null)
        : base(innerHandler ?? new HttpClientHandler())
    {
        this.authTokenService = authTokenService ?? throw new ArgumentNullException(nameof(IAuthTokenService));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // See if the request has an authorize header
        var auth = request.Headers.Authorization;
        if (auth == null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                authTokenService.GetToken());
        }

        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
        {
            return response;
        }

        var canRefrestToken = await TryRefreshToken(request, cancellationToken);
        if (!canRefrestToken)
        {
            return response;
        }

        request.Headers.Authorization = new AuthenticationHeaderValue(
            "Bearer", 
            authTokenService.GetToken());

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }

    private async Task<bool> TryRefreshToken(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var responseBody = new RefreshTokenRequest()
        {
            RefreshToken = authTokenService.GetTokenRefresh(),
        };
        var jsonBody = System.Text.Json.JsonSerializer.Serialize(responseBody);

        var uriString = $"{request?.RequestUri?.Scheme}://{request?.RequestUri?.Host}/api/token/v1/refresh_token";
        var refreshUri = new Uri(uriString);

        var refreshRequest = new HttpRequestMessage(HttpMethod.Post, refreshUri);
        refreshRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var responseRefreshToken  = await base.SendAsync(refreshRequest, cancellationToken).ConfigureAwait(false);

        if (!responseRefreshToken.IsSuccessStatusCode)
        {
            return false;
        }

        var responseToken = await responseRefreshToken.Content.ReadFromJsonAsync<RefreshTokenResponse>(options: null, cancellationToken);

        if (responseToken == null)
        {
            return false;
        }

        await authTokenService.SetToken(responseToken.Token, responseToken.RefreshToken);

        return true;
    }
}
