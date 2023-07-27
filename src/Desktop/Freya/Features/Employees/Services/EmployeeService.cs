using Models.Core.Common;

namespace Freya.Features.Employees.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeAPIService employeeAPIService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly IDialogService dialogService;
    private readonly ICurrentClinicService currentClinicService;
    private readonly ITranslatorService translatorService;
    private readonly ILogger<EmployeeService> logger;

    public EmployeeService(
        IRefitService refitService,
        ITaskHelperFactory taskHelperFactory,
        IDialogService dialogService,
        ILogger<EmployeeService> logger,
        ICurrentClinicService currentClinicService,
        ITranslatorService translatorService)
    {
        employeeAPIService = refitService.InitRefitInstance<IEmployeeAPIService>(isAutenticated: true);
        this.taskHelperFactory = taskHelperFactory;
        this.dialogService = dialogService;
        this.logger = logger;
        this.currentClinicService = currentClinicService;
        this.translatorService = translatorService;
    }

    public async Task<bool> DeleteEmployeeFromClinic(Guid employeeId, Guid clinicId)
    {
        if (clinicId == Guid.Empty)
        {
            return false;

        }

        var request = new UnassignEmployeeToClinicRequest()
        {
            ClinicId = clinicId,
            EmployeeId = employeeId,
        };

        var result = await taskHelperFactory.
                               CreateInternetAccessViewModelInstance(logger).
                               WithErrorHandling(OnSignInError).
                               TryExecuteAsync(
                               () => employeeAPIService.UnassignToClinic(request));

        return result.IsSuccess;
    }

    public async Task<PagedModel<EmployeeResponse>> GetEmployeesAsync(PaginationFilter paginationFilter)
    {
        var clinicId = currentClinicService.CurrentClinic?.Id ?? Guid.Empty;
        if (clinicId == Guid.Empty)
        {
            return PagedModel<EmployeeResponse>.Empty();

        }
        var result = await taskHelperFactory.
                               CreateInternetAccessViewModelInstance(logger).
                               WithErrorHandling(OnSignInError).
                               TryExecuteAsync(
                               () => employeeAPIService.GetEmployeesByClinicAsync(clinicId, paginationFilter));
        if (result.IsSuccess)
        {
            return result.Value;
        }

        return PagedModel<EmployeeResponse>.Empty();
    }

    private async Task<bool> OnSignInError(Exception arg)
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
