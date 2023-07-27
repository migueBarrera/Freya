namespace FreyaApi.Infrastructure.Models;

[Table("Employees")]
public class EmployeeTable
{
    [Key]
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Pass { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public Guid CompanyId { get; set; }

    public CompanyTable? Company { get; set; }

    public EmployeeRol Rol { get; set; } = EmployeeRol.CLINIC_OFFICER;

    public DateTime Created { get; set; } = DateTime.UtcNow;

    public ICollection<ClinicTable> Clinics { get; set; } = new List<ClinicTable>();
}
