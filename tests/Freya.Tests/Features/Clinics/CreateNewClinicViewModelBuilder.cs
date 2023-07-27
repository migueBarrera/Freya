using AppCenterServices;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freya.Tests.Features.Clinics;

internal class CreateNewClinicViewModelBuilder
{
    public readonly ISessionService sessionService;
    public readonly Mock<ICreateNewClinicService> createNewClinicService;
    public readonly Mock<INavigationService> navigationService;
    private readonly Mock<ITranslatorService> translatorService;
    private readonly Mock<ICurrentEmployeeService> currentEmployeeService;
    private readonly Mock<IDialogService> dialogService;
    private readonly Mock<AppCenterAnalitics> appCenterService;

    public CreateNewClinicViewModelBuilder()
    {
        this.createNewClinicService = new Mock<ICreateNewClinicService>();
        this.navigationService = new Mock<INavigationService>();
        this.currentEmployeeService = new Mock<ICurrentEmployeeService>();
        this.translatorService = new Mock<ITranslatorService>();
        this.sessionService = new SessionService();
        this.dialogService = new Mock<IDialogService>();
        appCenterService = new Mock<AppCenterAnalitics>();
    }

    internal CreateNewClinicViewModel Build()
    {
        return new CreateNewClinicViewModel(
            createNewClinicService.Object,
            navigationService.Object,
            currentEmployeeService.Object,
            dialogService.Object,
            translatorService.Object,
            appCenterService.Object);
    }

    internal CreateNewClinicViewModelBuilder WithEmployee(Employee employee)
    {
        currentEmployeeService.Setup(x => x.Employee).Returns(employee);
        return this;
    }
}
