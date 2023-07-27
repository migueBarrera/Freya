using ApiContract.Interfaces;
using Models.Core.Common;
using Models.Core.Employees;
using Refit;

namespace FreyaManager.Features.Employees;

public class EmployeeService
{
    private readonly IEmployeeAPIService employeeAPIService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly IDialogService dialogService;
    private readonly ITranslatorService translatorService;
    private readonly ILogger<EmployeeService> logger;

    public EmployeeService(
        IRefitService refitService,
        ITaskHelperFactory taskHelperFactory,
        ILogger<EmployeeService> logger,
        IDialogService dialogService,
        ITranslatorService translatorService)
    {
        employeeAPIService = refitService.InitRefitInstance<IEmployeeAPIService>(isAutenticated: true);
        this.taskHelperFactory = taskHelperFactory;
        this.logger = logger;
        this.dialogService = dialogService;
        this.translatorService = translatorService;
    }

    public async Task<bool> CreateEmployee(EmployeeSignUpRequest employee, Guid clinicId)
    {
        var result = await taskHelperFactory.
                            CreateInternetAccessViewModelInstance(logger).
                            TryExecuteAsync(
                            () => employeeAPIService.CreateEmployeeAsync(new EmployeeSignUpRequest()
                            {
                                Email = employee.Email,
                                LastName = employee.LastName,
                                Name = employee.Name,
                                Rol = employee.Rol,
                                CompanyId = employee.CompanyId,
                                ClinicId = clinicId,
                            }));

        return result.IsSuccess;
    }

    public async Task<PagedModel<EmployeeResponse>> GetEmployeesAsync(Guid clinicId, PaginationFilter paginationFilter)
    {
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
    
    public async Task<PagedModel<EmployeeResponse>> GetEmployeesByCompanyId(Guid companyId, PaginationFilter paginationFilter)
    {
        var result = await taskHelperFactory.
                               CreateInternetAccessViewModelInstance(logger).
                               WithErrorHandling(OnSignInError).
                               TryExecuteAsync(
                               () => employeeAPIService.GetEmployeesByCompanyAsync(companyId, paginationFilter));
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
