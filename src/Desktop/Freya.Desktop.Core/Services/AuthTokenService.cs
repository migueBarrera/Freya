namespace Freya.Desktop.Core.Services;

public class AuthTokenService : IAuthTokenService
{
    //De momento es sigleton, hay que usar prefrencias o algo asi y pasar a transaient
    private string token;
    private string refreshToken;

    public AuthTokenService()
    {
        token = string.Empty;
        refreshToken = string.Empty;
    }

    public string GetToken()
    {
        return token;
    }

    public string GetTokenRefresh()
    {
        return refreshToken;
    }

    public Task SetToken(string token, string refhresToken)
    {
        this.token = token;
        this.refreshToken = refhresToken;

        return Task.CompletedTask;
    }
}
