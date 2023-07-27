namespace Models.Core.Multimedia;

public class ImageMultimedia : BaseMultimedia
{
    public ImageMultimedia()
    {

    }

    public ImageMultimedia(BaseMultimedia baseMultimedia)
    {
        this.Id = baseMultimedia.Id;
        this.ClinicId = baseMultimedia.ClinicId;
        this.ClientId = baseMultimedia.ClientId;
        this.Uri = baseMultimedia.Uri;
        this.Size = baseMultimedia.Size;
    }
}
