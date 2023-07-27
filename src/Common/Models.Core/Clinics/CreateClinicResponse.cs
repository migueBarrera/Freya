namespace Models.Core.Clinics;

public class ClinicCreateResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public Guid CompanyId { get; set; }
}
