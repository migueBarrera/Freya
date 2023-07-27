using ApiContract.Interfaces;
using Models.Core.Common;
using Refit;

namespace FreyaManager.Services;

public class ClientService
{
    private readonly IClinicAPIService clinicApiService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly IDialogService dialogService;
    private readonly ITranslatorService translatorService;
    private readonly ILogger<ClientService> logger;

    public ClientService(
        IRefitService refitService,
        ITaskHelperFactory taskHelperFactory,
        IDialogService dialogService,
        ILogger<ClientService> logger,
        ITranslatorService translatorService)
    {
        this.clinicApiService = refitService.InitRefitInstance<IClinicAPIService>(isAutenticated: true);
        this.taskHelperFactory = taskHelperFactory;
        this.dialogService = dialogService;
        this.logger = logger;
        this.translatorService = translatorService;
    }

    public async Task<PagedModel<ClientResponse>> GetClients(Guid clinicId, PaginationFilter paginationFilter)
    {
        var result = await taskHelperFactory.
                               CreateInternetAccessViewModelInstance(logger).
                               WithErrorHandling(OnGetClientsError).
                               TryExecuteAsync(
                               () => clinicApiService.GetClientsByClinicAsync(clinicId, paginationFilter));

        if (!result.IsSuccess)
        {
            return PagedModel<ClientResponse>.Empty();
        }

        return result.Value;
    }

    private async Task<bool> OnGetClientsError(Exception arg)
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
