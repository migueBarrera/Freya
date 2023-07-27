namespace Models.Core.Clinics;

public class ClinicCreateRequest
{
    public string Name { get; set; } = string.Empty;

    public string Adress { get; set; } = string.Empty;

    public string NIF { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public Guid CompanyId { get; set; } = Guid.Empty;
}
