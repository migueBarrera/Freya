namespace FreyaApi.Infrastructure.Models;

[Table("clinicclient")]
public class ClinicClient
{
    public Guid ClinicId { get; set; }

    public ClinicTable? Clinic { get; set; }

    public Guid ClientId { get; set; }

    public ClientTable? Client { get; set; }

    public Guid SubscriptionPlanId { get; set; }

    public long SubscriptionPlanSize { get; set; }

    public DateTime Created { get; set; } = DateTime.UtcNow;
}
