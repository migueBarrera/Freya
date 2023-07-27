using ApiContract.Interfaces;
using Models.Core.Clients;

namespace Freya.Desktop.Core.Features.Clients.Services;

public class EditClientService
{
    private readonly IClientAPIService clientApiService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly IDialogService dialogService;
    private readonly ITranslatorService translatorService;
    private readonly ILogger<EditClientService> logger;

    public EditClientService(
        ITaskHelperFactory taskHelperFactory,
        IRefitService refitService,
        ILogger<EditClientService> logger,
        IDialogService dialogService,
        ITranslatorService translatorService)
    {
        this.taskHelperFactory = taskHelperFactory;
        clientApiService = refitService.InitRefitInstance<IClientAPIService>(isAutenticated: true);
        this.logger = logger;
        this.dialogService = dialogService;
        this.translatorService = translatorService;
    }

    public async Task<bool> EditClient(Guid id, ClientUpdateRequest client)
    {
        var result = await taskHelperFactory.
                                CreateInternetAccessViewModelInstance(logger).
                                WithErrorHandling(ErrorHandlerUpdateClient).
                                TryExecuteAsync(
                                () => clientApiService.UpdateClient(id, client));

        return result.IsSuccess;
    }

    private async Task<bool> ErrorHandlerUpdateClient(Exception arg)
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
