using ApiContract.Interfaces;

namespace FreyaMobile.Features.Sounds
{
    public class SoundsService
    {
        private readonly IClientAPIService clientApi;
        private readonly ICurrentUserService currentUserService;
        private readonly ITaskHelperFactory taskHelperFactory;
        private readonly ILogger<SoundsService> logger;

        public SoundsService(
            IRefitService refitService,
            ITaskHelperFactory taskHelperFactory,
            ILogger<SoundsService> logger,
            ICurrentUserService currentUserService)
        {
            clientApi = refitService.InitRefitInstance<IClientAPIService>(isAutenticated: true);
            this.taskHelperFactory = taskHelperFactory;
            this.logger = logger;
            this.currentUserService = currentUserService;
        }

        public async Task<IEnumerable<SoundModel>> GetSounds()
        {
            var user = await currentUserService.GetCurrentUserAsync();
            var clinic = currentUserService.GetCurrentClinic();

            var videosList = new List<SoundModel>();

            var response = await taskHelperFactory
                .CreateInternetAccessViewModelInstance(null)
                .TryExecuteAsync(() => clientApi.GetSounds(user!.Id, clinic!.Id));

            if (response.IsSuccess)
            {
                videosList = response.Value
                    .Select(SoundModel.ConvertTo)
                    .ToList();
            }

            return videosList;
        }
    }
}
