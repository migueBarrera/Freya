using Models.Core.Multimedia;

namespace FreyaMobile.Features.Videos;

public class VideoModel
{
    public Uri? Video { get; set; }

    public Uri? ThumbnailUrl { get; set; }

    public bool Available { get; set; }

    public static VideoModel ConvertTo(VideoMultimedia response) => new VideoModel()
    {
        Video = response.Uri,
        ThumbnailUrl = response.ThumnailUri,
        Available = true,
    };
}
