namespace Models.Core.Multimedia;

public class SoundMultimedia : BaseMultimedia
{
    public SoundMultimedia()
    {

    }

    public SoundMultimedia(BaseMultimedia baseMultimedia)
    {
        this.Id = baseMultimedia.Id;
        this.ClinicId = baseMultimedia.ClinicId;
        this.ClientId = baseMultimedia.ClientId;
        this.Uri = baseMultimedia.Uri;
        this.Size = baseMultimedia.Size;
    }
}
