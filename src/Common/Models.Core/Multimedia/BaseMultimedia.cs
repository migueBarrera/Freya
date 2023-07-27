namespace Models.Core.Multimedia;

public class BaseMultimedia
{
    public Guid Id { get; set; }

    public Uri Uri { get; set; } = new Uri("about:blank");

    public Guid ClientId { get; set; }

    public Guid ClinicId { get; set; }

    public long Size { get; set; }
}
