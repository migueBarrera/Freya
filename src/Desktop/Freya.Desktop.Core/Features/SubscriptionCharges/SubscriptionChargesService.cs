using ApiContract.Interfaces;
using Models.Core.Subscriptions;

namespace Freya.Desktop.Core.Features.SubscriptionCharges;

public class SubscriptionChargesService
{
    private readonly ISubscriptionPaymentAPIService subcriptionProductAPIService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly ILogger<SubscriptionChargesService> logger;

    public SubscriptionChargesService(
        IRefitService refitService,
        ITaskHelperFactory taskHelperFactory,
        ILogger<SubscriptionChargesService> logger)
    {
        this.subcriptionProductAPIService = refitService.InitRefitInstance<ISubscriptionPaymentAPIService>(isAutenticated: true);
        this.taskHelperFactory = taskHelperFactory;
        this.logger = logger;
    }

    public async Task<SubscriptionPaymentResponse?> CreateSubscriptionProduct(Guid paymentId)
    {
        var result = await taskHelperFactory.
                            CreateInternetAccessViewModelInstance(logger).
                            TryExecuteAsync(
                            () => subcriptionProductAPIService.GetSubscriptionPayment(paymentId));

        if (result.IsSuccess)
        {
            return result.Value;
        }

        return null;
    }
}
