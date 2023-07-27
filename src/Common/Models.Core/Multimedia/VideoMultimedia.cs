namespace Models.Core.Multimedia;

public class VideoMultimedia : BaseMultimedia
{
	public VideoMultimedia()
	{

	}

	public VideoMultimedia(BaseMultimedia baseMultimedia, Uri thumnailUri)
	{
		this.Id = baseMultimedia.Id;
		this.ClinicId = baseMultimedia.ClinicId;
		this.ClientId = baseMultimedia.ClientId;
		this.Uri = baseMultimedia.Uri;
		this.Size = baseMultimedia.Size;
		this.ThumnailUri = thumnailUri;
	}

    public Uri? ThumnailUri { get; set; }
}
