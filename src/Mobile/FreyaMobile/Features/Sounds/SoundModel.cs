using Models.Core.Multimedia;

namespace FreyaMobile.Features.Sounds;

public class SoundModel
{
    public Uri? Video { get; set; }

    public static SoundModel ConvertTo(BaseMultimedia response) => new SoundModel()
    {
        Video = response.Uri,
    };
}
