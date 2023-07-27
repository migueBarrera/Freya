namespace Models.Core.Clients;

public class ClientSignInResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;

    public IEnumerable<Clinic> Clinics { get; set; } = Enumerable.Empty<Clinic>();

    public DateTime Created { get; set; } = DateTime.UtcNow;
}
