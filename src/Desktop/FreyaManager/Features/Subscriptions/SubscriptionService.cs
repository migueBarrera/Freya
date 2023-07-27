using ApiContract.Interfaces;
using Models.Core.Subscriptions;
using Refit;

namespace FreyaManager.Features.Subscriptions;

public class SubscriptionService
{
    private readonly ISubscriptionProductAPIService subcriptionProductAPIService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly IDialogService dialogService;
    private readonly ITranslatorService translatorService;
    private readonly ILogger<SubscriptionService> logger;

    public SubscriptionService(
        IRefitService refitService,
        ITaskHelperFactory taskHelperFactory,
        ILogger<SubscriptionService> logger,
        ITranslatorService translatorService,
        IDialogService dialogService)
    {
        this.subcriptionProductAPIService = refitService.InitRefitInstance<ISubscriptionProductAPIService>(isAutenticated: true);
        this.taskHelperFactory = taskHelperFactory;
        this.logger = logger;
        this.translatorService = translatorService;
        this.dialogService = dialogService;
    }

    public async Task<bool> CreateSubscriptionProduct(string name, string description, long price, long? priceAnnual, string size, bool permitPromotionCodes)
    {
        var result = await taskHelperFactory.
                            CreateInternetAccessViewModelInstance(logger).
                            WithErrorHandling(OnErrorCreateSubscriptionProduct).
                            TryExecuteAsync(
                            () => subcriptionProductAPIService.CreateAsync(new SubscriptionProductCreateRequest()
                            {
                                IsActive = true,
                                Name = name,
                                Description = description,
                                PriceMonthly = price,
                                PriceAnnual = priceAnnual,
                                SizeOnBytes = size,
                                AllowPromotionCodes = permitPromotionCodes,
                            }));

        return result.IsSuccess;
    }

    private async Task<bool> OnErrorCreateSubscriptionProduct(Exception arg)
    {
        if (arg is ApiException apiException && apiException.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            await dialogService.ShowMessage(translatorService.Translate("error"),
                string.Empty);
            return true;
        }

        return false;
    }

    public async Task<IEnumerable<SubscriptionProductResponse>> GetSubscriptionProducts()
    {
        var result = await taskHelperFactory.
                            CreateInternetAccessViewModelInstance(logger).
                            TryExecuteAsync(
                            () => subcriptionProductAPIService.GetSubscriptionProducts());

        if (result.IsSuccess)
        {
            return result.Value;
        }
        else
        {
            return Enumerable.Empty<SubscriptionProductResponse>();
        }
    }
}
