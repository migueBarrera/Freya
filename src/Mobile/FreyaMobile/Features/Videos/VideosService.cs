using ApiContract.Interfaces;

namespace FreyaMobile.Features.Videos;

public class VideosService : IVideosService
{
    private readonly IClientAPIService videosApi;
    private readonly ICurrentUserService currentUserService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly ILogger<VideosService> logger;

    public VideosService(
        IRefitService refitService,
        ITaskHelperFactory taskHelperFactory,
        ILogger<VideosService> logger,
        ICurrentUserService currentUserService)
    {
        videosApi = refitService.InitRefitInstance<IClientAPIService>(isAutenticated: true);
        this.taskHelperFactory = taskHelperFactory;
        this.logger = logger;
        this.currentUserService = currentUserService;
    }

    public async Task<IEnumerable<VideoModel>> GetVideosAsync()
    {
        var user = await currentUserService.GetCurrentUserAsync();
        var clinic = currentUserService.GetCurrentClinic();

        var videosList = new List<VideoModel>();

        var response = await taskHelperFactory
            .CreateInternetAccessViewModelInstance(null)
            .TryExecuteAsync(() => videosApi.GetVideos(user!.Id, clinic!.Id));

        if (response.IsSuccess)
        {
            videosList = response.Value
                .Select(VideoModel.ConvertTo)
                .ToList();
        }

        return videosList;
    }
}
