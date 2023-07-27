using ApiContract.Interfaces;

namespace Freya.Desktop.Core.Features.Clients.Services;

public class MultimediaService
{
    private readonly IMultimediaAPIService multimediaAPIService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly IDialogService dialogService;
    private readonly ITranslatorService translatorService;
    private readonly ILogger<MultimediaService> logger;

    public MultimediaService(
        ILogger<MultimediaService> logger,
        IRefitService refitService,
        ITaskHelperFactory taskHelperFactory,
        IDialogService dialogService,
        ITranslatorService translatorService)
    {
        this.logger = logger;
        this.taskHelperFactory = taskHelperFactory;
        multimediaAPIService = refitService.InitRefitInstance<IMultimediaAPIService>(isAutenticated: true);
        this.dialogService = dialogService;
        this.translatorService = translatorService;
    }

    public async Task<bool> Delete(Guid id)
    {
        var result = await taskHelperFactory.
                           CreateInternetAccessViewModelInstance(logger).
                           WithErrorHandling(OnDeleteMultimediaError).
                           TryExecuteAsync(
                           () => multimediaAPIService.Delete(id));

        return result.IsSuccess;
    }

    private async Task<bool> OnDeleteMultimediaError(Exception arg)
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
