using Freya.Features.Clinics.Models;
using Models.Core.Clinics;

namespace Freya.Tests.Features.Clinics;

internal class ClinicDetailViewModelBuilder
{
    private readonly Mock<IDialogService> dialogService;
    private readonly Mock<IServiceProvider> serviceProvider;
    public readonly Mock<ISessionService> sessionService;
    private readonly Mock<ICurrentClinicService> currentClinicService;
    private readonly Mock<ICurrentEmployeeService> currentEmployeeService;
    private readonly Mock<INavigationService> navigationService;
    private readonly Mock<ITranslatorService> translatorService;
    private readonly Mock<AppCenterAnalitics> appCenterAnalitics;
    public readonly Mock<IClinicDetailService> clinicDetailService;
    private readonly IEmployeeRolService employeeRolService;

    public ClinicDetailViewModelBuilder()
    {
        dialogService = new Mock<IDialogService>();
        sessionService = new Mock<ISessionService>();
        serviceProvider = new Mock<IServiceProvider>();
        currentEmployeeService = new Mock<ICurrentEmployeeService>();
        employeeRolService = new EmployeeRolService();
        currentClinicService = new Mock<ICurrentClinicService>();
        navigationService = new Mock<INavigationService>();
        translatorService = new Mock<ITranslatorService>();
        appCenterAnalitics = new Mock<AppCenterAnalitics>();
        clinicDetailService = new Mock<IClinicDetailService>();
    }

    internal ClinicDetailViewModel Build()
    {
        return new ClinicDetailViewModel(
            sessionService.Object,
            clinicDetailService.Object,
            currentClinicService.Object,
            serviceProvider.Object,
            dialogService.Object,
            navigationService.Object,
            employeeRolService,
            currentEmployeeService.Object,
            translatorService.Object,
            new Mock<ClinicDetailEmployeesViewModel>(null, null).Object,
            new Mock<ClinicDetailClientsViewModel>(null, null, null, null).Object,
            appCenterAnalitics.Object);
    }

    internal ClinicDetailViewModelBuilder WithRol(EmployeeRol employeeRol)
    {
        currentEmployeeService.Setup(s => s.Employee).Returns(new Employee()
        {
            Rol = employeeRol,
        });

        return this;
    }

    internal ClinicDetailViewModelBuilder WithClinic(ClinicDetailResponse clinic)
    {
        clinicDetailService.Setup(x=> x.GetClinic(It.IsAny<Guid>())).ReturnsAsync(clinic);
        return this;
    }

    internal ClinicDetailViewModelBuilder SaveOnSession<T>(string SessionKey, object clientResponse)
    {
        sessionService.Setup(x => x.Get<T>(It.Is<string>(y => y == SessionKey))).Returns((T)clientResponse);

        return this;
    }
}
