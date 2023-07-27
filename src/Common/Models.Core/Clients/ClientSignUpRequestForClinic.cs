namespace Models.Core.Clients;

public class ClientSignUpRequestForClinic
{
    public string Name { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public Guid ClinicId { get; set; } = Guid.Empty;
}
