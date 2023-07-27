namespace FreyaApi.Infrastructure.Models;

[Table("Videos")]
public class VideoTable : BaseMultimedia
{
    public Uri? ThumnailUri { get; set; }
}
