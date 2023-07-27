namespace Freya.Features.Clients.Services;

public class CreateClientService : ICreateClientService
{
    private readonly IClientAPIService clientAPIService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly IDialogService dialogService;
    private readonly ITranslatorService translatorService;
    private readonly ILogger<CreateClientService> logger;

    public CreateClientService(
        IRefitService refitService,
        ITaskHelperFactory taskHelperFactory,
        ILogger<CreateClientService> logger,
        IDialogService dialogService,
        ITranslatorService translatorService)
    {
        clientAPIService = refitService.InitRefitInstance<IClientAPIService>(isAutenticated: true);
        this.taskHelperFactory = taskHelperFactory;
        this.logger = logger;
        this.dialogService = dialogService;
        this.translatorService = translatorService;
    }

    public async Task<bool> CreateClient(Client client, Guid clinicId)
    {
        var result = await taskHelperFactory.
                            CreateInternetAccessViewModelInstance(logger).
                            WithErrorHandling(OnCreateClientError).
                            TryExecuteAsync(
                            () => clientAPIService.SignUpForClinicAsync(new ClientSignUpRequestForClinic()
                            {
                                Email = client.Email,
                                LastName = client.LastName,
                                Name = client.Name,
                                Phone = client.Phone,
                                ClinicId = clinicId,
                            }));

        return result.IsSuccess;
    }

    private async Task<bool> OnCreateClientError(Exception arg)
    {
        if (ApiExceptionHelper.IsACustomException(arg, out ErrorModel errorModel))
        {
            if (errorModel.ErrorCode == ErrorType.CLINIC_NOT_HAS_A_VALID_SUBCRIPTION)
            {
                await dialogService.ShowMessage(
                    translatorService.Translate("dialog_new_client_clinic_not_subscription_valid_title"),
                    translatorService.Translate("dialog_new_client_clinic_not_subscription_valid_body"));
                return true;
            }
        }

        return false;
    }
}
