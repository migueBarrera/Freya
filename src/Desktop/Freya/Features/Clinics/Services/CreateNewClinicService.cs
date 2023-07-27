namespace Freya.Features.Clinics.Services;

public class CreateNewClinicService : ICreateNewClinicService
{
    private readonly IClinicAPIService clinicAPIService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly IDialogService dialogService;
    private readonly ITranslatorService translatorService;
    private readonly ILogger<CreateNewClinicService> logger;

    public CreateNewClinicService(
        ITaskHelperFactory taskHelperFactory,
        IRefitService refitService,
        ILogger<CreateNewClinicService> logger,
        IDialogService dialogService,
        ITranslatorService translatorService)
    {
        this.taskHelperFactory = taskHelperFactory;
        clinicAPIService = refitService.InitRefitInstance<IClinicAPIService>(isAutenticated: true);
        this.logger = logger;
        this.dialogService = dialogService;
        this.translatorService = translatorService;
    }

    public async Task<ClinicCreateResponse?> CreateClinic(ClinicCreateRequest clinic)
    {
        var result = await taskHelperFactory.
                                CreateInternetAccessViewModelInstance(logger).
                                WithErrorHandling(ErrorHandlerCreateClinic).
                                TryExecuteAsync(
                                () => clinicAPIService.CreateAsync(clinic));
        if (result.IsSuccess)
        {
            return result.Value;
        }

        return null;
    }

    private async Task<bool> ErrorHandlerCreateClinic(Exception arg)
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
