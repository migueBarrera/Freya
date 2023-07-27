namespace FreyaApi.Infrastructure.Models;

[Table("Companies")]
public class CompanyTable
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<EmployeeTable> Employees { get; set; } = new List<EmployeeTable>();

    public ICollection<ClinicTable> Clinics { get; set; } = new List<ClinicTable>();

    public ICollection<SubscriptionPlanTable> SubscriptionPlans { get; set; } = new List<SubscriptionPlanTable>();

    public DateTime Created { get; set; } = DateTime.UtcNow;
}
