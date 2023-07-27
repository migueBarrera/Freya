namespace FreyaApi.Repository.Interfaces;

public interface IVideoRepository
{
    Task<VideoTable?> GetVideo(Guid videoId);

    Task<IEnumerable<VideoTable>> GetVideos(Guid clientId, Guid clinicId);

    bool VideoExist(Guid id);

    Task<Guid> AddVideo(VideoTable video);

    Task<bool> RemoveVideo(Guid videoId);

    Task<bool> RemoveVideos(IEnumerable<Guid> videoId);
}
