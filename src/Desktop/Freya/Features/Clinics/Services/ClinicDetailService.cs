namespace Freya.Features.Clinics.Services;

public class ClinicDetailService : IClinicDetailService
{
    private readonly IClinicAPIService clinicAPIService;
    private readonly ISubscriptionPaymentAPIService subscriptionPaymentAPIService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly ILogger<ClinicDetailService> logger;

    public ClinicDetailService(IRefitService refitService, ITaskHelperFactory taskHelperFactory, ILogger<ClinicDetailService> logger)
    {
        this.clinicAPIService = refitService.InitRefitInstance<IClinicAPIService>(isAutenticated: true);
        this.subscriptionPaymentAPIService = refitService.InitRefitInstance<ISubscriptionPaymentAPIService>(isAutenticated: true);
        this.taskHelperFactory = taskHelperFactory;
        this.logger = logger;
    }

    public async Task<bool> CancelSubscription(Guid id)
    {
        var result = await taskHelperFactory.
                            CreateInternetAccessViewModelInstance(logger).
                            TryExecuteAsync(
                            () => subscriptionPaymentAPIService.CancelSubscription(id));

        if (result.IsSuccess)
        {
            return result.Value;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> DeleteClinic(Guid id)
    {
        var result = await taskHelperFactory.
                            CreateInternetAccessViewModelInstance(logger).
                            TryExecuteAsync(
                            () => clinicAPIService.Delete(id));

        return result.IsSuccess;
    }

    public async Task<ClinicDetailResponse> GetClinic(Guid id)
    {
        var result = await taskHelperFactory.
                            CreateInternetAccessViewModelInstance(logger).
                            TryExecuteAsync(
                            () => clinicAPIService.GetAsync(id));

        if (result.IsSuccess)
        {
            return result.Value;
        }
        else
        {
            return new ClinicDetailResponse();
        }
    }
}
