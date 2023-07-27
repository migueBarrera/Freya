namespace Freya.Features.Employees.Services;

public class CreateNewEmployeeService : ICreateNewEmployeeService
{
    private readonly IEmployeeAPIService employeeAPIService;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly ILogger<CreateNewEmployeeService> logger;

    public CreateNewEmployeeService(
        IRefitService refitService,
        ITaskHelperFactory taskHelperFactory,
        ILogger<CreateNewEmployeeService> logger)
    {
        employeeAPIService = refitService.InitRefitInstance<IEmployeeAPIService>(isAutenticated: true);
        this.taskHelperFactory = taskHelperFactory;
        this.logger = logger;
    }

    public async Task<bool> CreateEmployee(Employee employee, Guid clinicId)
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
                                Pass = string.Empty,
                                CompanyId = employee.CompanyId,
                                ClinicId = clinicId,
                            }));

        return result.IsSuccess;
    }
}
