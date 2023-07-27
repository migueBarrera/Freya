namespace Models.Core.Clinics;

public class Clinic
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public Guid CompanyId { get; set; }

    public ICollection<EmployeeResponse> Employees { get; set; } = new List<EmployeeResponse>();

    public override string ToString()
    {
        return Name;
    }
}
