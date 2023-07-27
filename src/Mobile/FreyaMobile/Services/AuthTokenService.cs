namespace FreyaMobile.Services;

internal class AuthTokenService : IAuthTokenService
{
    const string tokenPreferencesKey = nameof(tokenPreferencesKey);
    const string tokenRefreshPreferencesKey = nameof(tokenRefreshPreferencesKey);

    public AuthTokenService()
    {
    }

    public string GetToken()
    {
        return Preferences.Default.Get<string>(tokenPreferencesKey, string.Empty);
    }

    public string GetTokenRefresh()
    {
        return Preferences.Default.Get<string>(tokenRefreshPreferencesKey, string.Empty);
    }

    public Task SetToken(string token, string refhresToken)
    {
        Preferences.Default.Set(tokenPreferencesKey, token);
        Preferences.Default.Set(tokenRefreshPreferencesKey, refhresToken);

        return Task.CompletedTask;
    }
}