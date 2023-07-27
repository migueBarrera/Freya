namespace FreyaApi.Infrastructure.Models;

[Table("Clients")]
public class ClientTable
{
    [Key]
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Pass { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public Uri? Image { get; set; }

    public DateTime Created { get; set; } = DateTime.UtcNow;

    public ICollection<ClinicClient> ClinicClients { get; set; } = new List<ClinicClient>();

    public ICollection<VideoTable> Videos { get; set; } = new List<VideoTable>();

    public ICollection<UltrasoundTable> Ultrasounds { get; set; } = new List<UltrasoundTable>();

    public ICollection<SoundTable> Sounds { get; set; } = new List<SoundTable>();
}
