namespace Freya.Features.Faq;

public class FaqService
{
    private readonly IFaqAPIService faqAPIService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly IDialogService dialogService;
    private readonly ILogger<FaqService> logger;

    public FaqService(
        IRefitService refitService,
        ITaskHelperFactory taskHelperFactory,
        ILogger<FaqService> logger,
        IDialogService dialogService)
    {
        faqAPIService = refitService.InitRefitInstance<IFaqAPIService>(isAutenticated: true);
        this.taskHelperFactory = taskHelperFactory;
        this.logger = logger;
        this.dialogService = dialogService;
    }

    public async Task<IEnumerable<FAQ>> GetAll()
    {
        var result = await taskHelperFactory.
                               CreateInternetAccessViewModelInstance(logger).
                               TryExecuteAsync(
                               () => faqAPIService.GetAll());

        if (result.IsSuccess)
        {
            return result.Value;
        }

        return Enumerable.Empty<FAQ>();
    }
}
