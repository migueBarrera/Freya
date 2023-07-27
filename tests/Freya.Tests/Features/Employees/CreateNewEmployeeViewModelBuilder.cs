using Freya.Features.Employees;
using Freya.Features.Employees.Services;
using Freya.Services;
using Models.Core.Employees;

namespace Freya.Tests.Features.Employees;

internal class CreateNewEmployeeViewModelBuilder
{
    private readonly Mock<ICurrentEmployeeService> currentEmployee;
    private readonly Mock<IDialogService> dialogService;
    private readonly Mock<ISessionService> sessionService;
    private readonly Mock<ICreateNewEmployeeService> createNewEmployeeService;
    private readonly Mock<INavigationService> navigationService;
    private readonly Mock<ITranslatorService> translatorService;
    private readonly EmployeeRolService employeeRolService;
    private readonly Mock<AppCenterAnalitics> appCenterAnalitics;

    public CreateNewEmployeeViewModelBuilder()
    {
        currentEmployee = new Mock<ICurrentEmployeeService>();
        dialogService = new Mock<IDialogService>();
        sessionService = new Mock<ISessionService>();
        createNewEmployeeService = new Mock<ICreateNewEmployeeService>();
        navigationService = new Mock<INavigationService>();
        translatorService = new Mock<ITranslatorService>();
        employeeRolService = new EmployeeRolService();
        appCenterAnalitics = new Mock<AppCenterAnalitics>();
    }

    internal CreateNewEmployeeViewModel Build()
    {
        return new CreateNewEmployeeViewModel(
            currentEmployee.Object,
            dialogService.Object,
            sessionService.Object,
            createNewEmployeeService.Object,
            navigationService.Object,
            translatorService.Object,
            employeeRolService,
            appCenterAnalitics.Object);
    }

    internal CreateNewEmployeeViewModelBuilder WithRol(EmployeeRol employeeRol)
    {
        currentEmployee.Setup(s => s.Employee).Returns(new Employee()
        {
            Rol = employeeRol,
        });

        return this;
    }
}
