namespace ApiContract.Refit;

public interface IAuthTokenService
{
    Task SetToken(string token, string refhresToken);

    string GetToken();
    string GetTokenRefresh();
}
