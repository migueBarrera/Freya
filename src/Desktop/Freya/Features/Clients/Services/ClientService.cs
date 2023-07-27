using Models.Core.Common;

namespace Freya.Features.Clients.Services;

public class ClientService
{
    private readonly IClinicAPIService clinicApiService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly IDialogService dialogService;
    private readonly ICurrentClinicService currentClinicService;
    private readonly ITranslatorService translatorService;
    private readonly ILogger<ClientService> logger;

    public ClientService(
        IRefitService refitService,
        ITaskHelperFactory taskHelperFactory,
        IDialogService dialogService,
        ILogger<ClientService> logger,
        ICurrentClinicService currentClinicService,
        ITranslatorService translatorService)
    {
        this.clinicApiService = refitService.InitRefitInstance<IClinicAPIService>(isAutenticated: true);
        this.taskHelperFactory = taskHelperFactory;
        this.dialogService = dialogService;
        this.logger = logger;
        this.currentClinicService = currentClinicService;
        this.translatorService = translatorService;
    }

    public async Task<PagedModel<ClientResponse>> GetClients(PaginationFilter paginationFilter)
    {
        if (currentClinicService.CurrentClinic == null)
        {
            return PagedModel<ClientResponse>.Empty();
        }

        var clinicId = currentClinicService.CurrentClinic.Id;
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
