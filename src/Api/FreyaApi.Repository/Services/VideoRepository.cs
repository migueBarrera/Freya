namespace FreyaApi.Repository.Services;

public class VideoRepository : BaseRepository, IVideoRepository
{

    public VideoRepository(DatabaseContext databaseContext)
         : base(databaseContext)
    {
    }

    public async Task<IEnumerable<VideoTable>> GetVideos(Guid clientId, Guid clinicId)
    {
        var videos = await context.Videos?
            .Where(x => x.ClientId == clientId && x.ClinicId == clinicId)
            .OrderByDescending(x => x.Created)
            .AsNoTracking()
            .ToListAsync()!;

        if (videos == null)
        {
            return Enumerable.Empty<VideoTable>();
        }

        return videos;
    }

    public async Task<VideoTable?> GetVideo(Guid videoId)
    {
        var item = await context
        .Videos?
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == videoId)!;

        return item;
    }

    public bool VideoExist(Guid id)
    {
        return context.Videos?.Any(x => x.Id == id) ?? false;
    }

    public async Task<Guid> AddVideo(VideoTable video)
    {
        context.Videos?.Add(video);
        await context.SaveChangesAsync();
        return video.Id;
    }

    public async Task<bool> RemoveVideo(Guid videoId)
    {

        var item = context.Videos?.FirstOrDefault(x => x.Id == videoId);
        if (item == null)
        {
            return false;
        }

        context.Videos?.Remove(item);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveVideos(IEnumerable<Guid> videoId)
    {

        var items = context.Videos?
                   .Where(x => videoId.Contains(x.Id))
                   .ToList();

        bool hasAnyItem = items?.Any() ?? false;
        if (!hasAnyItem)
        {
            return false;
        }

        await context.SaveChangesAsync();

        return true;
    }
}
