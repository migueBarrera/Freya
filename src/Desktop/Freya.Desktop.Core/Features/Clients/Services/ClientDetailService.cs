using ApiContract.Interfaces;
using Models.Core.Clients;

namespace Freya.Desktop.Core.Features.Clients.Services;

public class ClientDetailService : IClientDetailService
{
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly ILogger<ClientDetailService> logger;
    private readonly IClientAPIService clientApiService;
    private IDialogService dialogService;
    private readonly ITranslatorService translatorService;

    public ClientDetailService(
        ILogger<ClientDetailService> logger,
        IRefitService refitService,
        ITaskHelperFactory taskHelperFactory,
        IDialogService dialogService,
        ITranslatorService translatorService)
    {
        this.logger = logger;
        this.taskHelperFactory = taskHelperFactory;
        clientApiService = refitService.InitRefitInstance<IClientAPIService>(isAutenticated: true);
        this.dialogService = dialogService;
        this.translatorService = translatorService;
    }

    public async Task<bool> DeleteClient(Guid clientId, Guid clinicId)
    {
        var result = await taskHelperFactory.
                               CreateInternetAccessViewModelInstance(logger).
                               WithErrorHandling(OnGetUltrasoundError).
                               TryExecuteAsync(
                               () => clientApiService.DeleteClientAsosiation(clientId, clinicId));

        return result.IsSuccess;
    }

    public async Task<ClientDetailResponse> GetClientDetails(Guid clientId, Guid clinicId)
    {
        var result = await taskHelperFactory.
                               CreateInternetAccessViewModelInstance(logger).
                               WithErrorHandling(OnGetUltrasoundError).
                               TryExecuteAsync(
                               () => clientApiService.GetAsync(clientId, clinicId));
        if (result.IsSuccess)
        {
            return result.Value;
        }

        return new ClientDetailResponse();
    }

    public async Task<bool> UpdateClientPlan(Guid id, Guid clinicId)
    {
        var request = new ClientUpdatePlanRequest()
        {
            ClientId = id,
            ClinicId = clinicId,
        };

        var result = await taskHelperFactory.
                               CreateInternetAccessViewModelInstance(logger).
                               WithErrorHandling(OnGetUltrasoundError).
                               TryExecuteAsync(
                               () => clientApiService.UpdateClientPlan(request));

        return result.IsSuccess;
    }

    private async Task<bool> OnGetUltrasoundError(Exception arg)
    {
        if (arg is ApiException apiException && apiException.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            await dialogService.ShowMessage(translatorService.Translate("error"),
                string.Empty);
            return true;
        }

        return false;
    }
}
