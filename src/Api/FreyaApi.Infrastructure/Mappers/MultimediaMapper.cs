using Models.Core.Multimedia;

namespace FreyaApi.Infrastructure.Mappers;

public static class MultimediaMapper
{
    public static VideoMultimedia ConverTo(VideoTable item)
    {
        return new VideoMultimedia()
        {
            ClientId = item.ClientId,
            Id = item.Id,
            ClinicId = item.ClinicId,
            Uri = item.Uri,
            Size = item.Size,
            ThumnailUri = item.ThumnailUri,
        };
    }
    
    public static ImageMultimedia ConverTo(UltrasoundTable item)
    {
        return new ImageMultimedia()
        {
            ClientId = item.ClientId,
            Id = item.Id,
            ClinicId = item.ClinicId,
            Uri = item.Uri,
            Size = item.Size,
        };
    }
    
    public static SoundMultimedia ConverTo(SoundTable item)
    {
        return new SoundMultimedia()
        {
            ClientId = item.ClientId,
            Id = item.Id,
            ClinicId = item.ClinicId,
            Uri = item.Uri,
            Size = item.Size,
        };
    }
}
