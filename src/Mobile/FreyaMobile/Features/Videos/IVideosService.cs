namespace FreyaMobile.Features.Videos;

public interface IVideosService
{
    Task<IEnumerable<VideoModel>> GetVideosAsync();
}
