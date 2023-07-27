namespace FreyaApi.Infrastructure.Models;

public class BaseMultimedia
{
    [Key]
    public Guid Id { get; set; }

    public Uri Uri { get; set; } = new Uri("about:blank");

    public Guid ClientId { get; set; }

    public ClientTable? Client { get; set; }

    public Guid ClinicId { get; set; }

    public ClinicTable? Clinic { get; set; }

    public long Size { get; set; }

    public DateTime Created { get; set; } = DateTime.UtcNow;
}
