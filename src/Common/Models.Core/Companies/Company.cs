namespace Models.Core.Companies;

public class Company
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public IEnumerable<EmployeeResponse> Employees { get; set; } = Enumerable.Empty<EmployeeResponse>();

    public IEnumerable<ClinicResponse> Clinics { get; set; } = Enumerable.Empty<ClinicResponse>();

    public DateTime Created { get; set; } = DateTime.UtcNow;
}
