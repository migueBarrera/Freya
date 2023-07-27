namespace FreyaApi.Infrastructure.Models;

[Table("Clinics")]
public class ClinicTable
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Adress { get; set; } = string.Empty;

    public string NIF { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public Guid CompanyId { get; set; }

    public CompanyTable? Company { get; set; }

    public ICollection<EmployeeTable> Employees { get; set; } = new List<EmployeeTable>();

    public ICollection<ClinicClient> ClinicClients { get; set; } = new List<ClinicClient>();

    public ICollection<VideoTable> Videos { get; set; } = new List<VideoTable>();

    public ICollection<UltrasoundTable> Ultrasounds { get; set; } = new List<UltrasoundTable>();

    public ICollection<SoundTable> Sounds { get; set; } = new List<SoundTable>();

    public ICollection<SubscriptionPaymentTable> SubscriptionPayments { get; set; } = new List<SubscriptionPaymentTable>();

    public DateTime Created { get; set; } = DateTime.UtcNow;
}
