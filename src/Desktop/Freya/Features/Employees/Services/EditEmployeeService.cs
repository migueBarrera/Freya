namespace Freya.Features.Employees.Services;

public class EditEmployeeService : IEditEmployeeService
{
    private readonly IEmployeeAPIService employeeAPIService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly ILogger<EditEmployeeService> logger;

    public EditEmployeeService(
            IRefitService refitService,
            ITaskHelperFactory taskHelperFactory,
            ILogger<EditEmployeeService> logger)
    {
        employeeAPIService = refitService.InitRefitInstance<IEmployeeAPIService>(isAutenticated: true);
        this.taskHelperFactory = taskHelperFactory;
        this.logger = logger;
    }

    public async Task<bool> SaveEmployeeData(EmployeeUpdateRequest employeeUpdate)
    {
        var result = await taskHelperFactory.
                                CreateInternetAccessViewModelInstance(logger).
                                TryExecuteAsync(
                                () => employeeAPIService.UpdateEmployeeData(employeeUpdate));

        return result.IsSuccess;
    }
}
