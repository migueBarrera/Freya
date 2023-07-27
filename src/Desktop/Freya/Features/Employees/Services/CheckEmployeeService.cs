namespace Freya.Features.Employees.Services;

internal class CheckEmployeeService : ICheckEmployeeService
{
    private readonly IEmployeeAPIService employeeAPIService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly IDialogService dialogService;
    private readonly ITranslatorService translatorService;
    private readonly ILogger<CheckEmployeeService> logger;

    public CheckEmployeeService(
        IRefitService refitService,
        ITaskHelperFactory taskHelperFactory,
        IDialogService dialogService,
        ILogger<CheckEmployeeService> logger,
        ITranslatorService translatorService)
    {
        employeeAPIService = refitService.InitRefitInstance<IEmployeeAPIService>(isAutenticated: true);
        this.taskHelperFactory = taskHelperFactory;
        this.dialogService = dialogService;
        this.logger = logger;
        this.translatorService = translatorService;
    }

    public async Task<Result<bool>> AssignEmployeeToSelectedClinic(string email, Guid clinicId)
    {
        Result<bool> result = Error();
        var response = await taskHelperFactory.
                               CreateInternetAccessViewModelInstance(logger).
                               WithErrorHandling(OnCheckEmployeeStateError).
                               TryExecuteAsync(
                               () => employeeAPIService.AssignToClinic(new AssignEmployeeToClinicRequest()
                               {
                                   EmployeeEmail = email,
                                   ClinicId = clinicId,
                               }));

        if (response.IsSuccess)
        {
            result = Ok(true);
        }

        return result;
    }

    public async Task<Result<NewEmployeeState>> CheckEmployeeState(string email, Guid clinicId)
    {
        Result<NewEmployeeState> result = Error();
        var response = await taskHelperFactory.
                               CreateInternetAccessViewModelInstance(logger).
                               WithErrorHandling(OnCheckEmployeeStateError).
                               TryExecuteAsync(
                               () => employeeAPIService.CheckEmployeeStateForRegister(new CheckEmployeeStateForRegisterResquest()
                               {
                                   EmployeeEmail = email,
                                   ClinicId = clinicId,
                               }));

        if (response.IsSuccess)
        {
            result = Ok(response.Value.NewEmployeeState);
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
}
