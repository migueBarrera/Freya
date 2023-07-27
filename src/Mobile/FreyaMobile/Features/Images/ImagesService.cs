using ApiContract.Interfaces;
using FreyaMobile.Features.Images.Models;

namespace FreyaMobile.Features.Images.Services;

public class ImagesService : IImagesService
{
    private readonly IClientAPIService imagesApi;
    private readonly ICurrentUserService currentUserService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly ILogger<ImagesService> logger;

    public ImagesService(
        IRefitService refitService,
        ICurrentUserService currentUserService,
        ITaskHelperFactory taskHelperFactory,
        ILogger<ImagesService> logger)
    {
        this.imagesApi = refitService.InitRefitInstance<IClientAPIService>(isAutenticated: true);
        this.currentUserService = currentUserService;
        this.taskHelperFactory = taskHelperFactory;
        this.logger = logger;
    }

    public async Task<IEnumerable<ImageModel>> GetImagesAsync()
    {
        var user = await currentUserService.GetCurrentUserAsync();
        var clinic = currentUserService.GetCurrentClinic();

        var imagesList = new List<ImageModel>();

        var response = await this.taskHelperFactory
            .CreateInternetAccessViewModelInstance(null)
            .TryExecuteAsync(() => imagesApi.GetUltrasounds(user!.Id, clinic!.Id));

        if (response.IsSuccess)
        {
            imagesList = response.Value
                .Select(ImagesResponse.ConvertTo)
                .ToList();
        }

        return imagesList;
    }
}
