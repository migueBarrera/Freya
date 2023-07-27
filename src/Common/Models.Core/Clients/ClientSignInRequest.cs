namespace Models.Core.Clients;

public class ClientSignInRequest
{
    public string Email { get; set; } = string.Empty;

    public string Pass { get; set; } = string.Empty;

    public string PushToken { get; set; } = string.Empty;
}
