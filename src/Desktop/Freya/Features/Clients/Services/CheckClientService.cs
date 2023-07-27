namespace Freya.Features.Clients.Services;

public class CheckClientService : ICheckClientService
{
    private readonly IClientAPIService clientAPIService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly IDialogService dialogService;
    private readonly ITranslatorService translatorService;
    private readonly ILogger<CheckClientService> logger;

    public CheckClientService(
        IRefitService refitService,
        ITaskHelperFactory taskHelperFactory,
        IDialogService dialogService,
        ILogger<CheckClientService> logger,
        ITranslatorService translatorService)
    {
        clientAPIService = refitService.InitRefitInstance<IClientAPIService>(isAutenticated: true);
        this.taskHelperFactory = taskHelperFactory;
        this.dialogService = dialogService;
        this.logger = logger;
        this.translatorService = translatorService;
    }

    public async Task<Result<bool>> AssignClientToSelectedClinic(string email, Guid clinicId)
    {
        Result<bool> result = Error();
        var response = await taskHelperFactory.
                               CreateInternetAccessViewModelInstance(logger).
                               WithErrorHandling(OnAssignClientToClinicError).
                               TryExecuteAsync(
                               () => clientAPIService.AssignToClinic(new AssignClientToClinicRequest()
                               {
                                   ClientEmail = email,
                                   ClinicId = clinicId,
                               }));

        if (response.IsSuccess)
        {
            result = Ok(true);
        }

        return result;
    }

    public async Task<Result<NewClientState>> CheckNewClientState(string email, Guid clinicId)
    {
        Result<NewClientState> result = Error();
        var response = await taskHelperFactory.
                               CreateInternetAccessViewModelInstance(logger).
                               WithErrorHandling(OnCheckEmployeeStateError).
                               TryExecuteAsync(
                               () => clientAPIService.CheckEmployeeStateForRegister(new CheckClientStateForRegisterResquest()
                               {
                                   ClientEmail = email,
                                   ClinicId = clinicId,
                               }));

        if (response.IsSuccess)
        {
            result = Ok(response.Value.NewClientState);
        }

        return result;
    }

    private async Task<bool> OnCheckEmployeeStateError(Exception arg)
    {
        if (arg is ApiException apiException && apiException.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            await dialogService.ShowMessage(translatorService.Translate("error"),
                string.Empty);
            return true;
        }

        return false;
    }
    
    private async Task<bool> OnAssignClientToClinicError(Exception arg)
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
