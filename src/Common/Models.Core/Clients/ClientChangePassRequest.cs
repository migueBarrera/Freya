namespace Models.Core.Clients;

public class ClientChangePassRequest
{
    public Guid ClientId { get; set; }

    public string ActualPassword { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
