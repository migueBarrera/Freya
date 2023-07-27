namespace Models.Core.AppManagers;

public class AppManagerSignInResponse
{
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;
}
