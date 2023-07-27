namespace Models.Core.Multimedia;

public class CreateMultimediaRequest
{
    public CreateMultimediaRequest(Uri uri, Guid clinicId, Guid clientId, MultimediaType type, long size, Uri? thumnailUri)
    {
        Uri = uri;
        ClinicId = clinicId;
        ClientId = clientId;
        Type = type;
        Size = size;
        ThumnailUri = thumnailUri;
    }

    public Uri Uri { get; set; }

    public Uri? ThumnailUri { get; set; }

    public Guid ClinicId { get; set; }

    public Guid ClientId { get; set; }

    public MultimediaType Type{ get; set; }

    public long Size{ get; set; }
}
