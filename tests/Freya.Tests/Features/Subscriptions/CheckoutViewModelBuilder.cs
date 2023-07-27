using Freya.Features.SignIn;
using Freya.Features.Subscriptions;
using Freya.Services;

namespace Freya.Tests.Features.Subscriptions;

internal class CheckoutViewModelBuilder
{
    public readonly ISessionService sessionService;
    public readonly Mock<ISignInService> signInService;
    public readonly Mock<INavigationService> navigationService;
    private readonly Mock<ICurrentEmployeeService> currentEmployeeService;
    private readonly Mock<ICurrentClinicService> currentClinicService;
    private readonly Mock<AppCenterAnalitics> appCenterAnalitics;

    public CheckoutViewModelBuilder()
    {
        this.signInService = new Mock<ISignInService>();
        this.navigationService = new Mock<INavigationService>();
        this.currentEmployeeService = new Mock<ICurrentEmployeeService>();
        this.sessionService = new SessionService();
        this.currentClinicService = new Mock<ICurrentClinicService>();
        this.appCenterAnalitics = new Mock<AppCenterAnalitics>();
    }

    internal CheckoutViewModel Build()
    {
        return new CheckoutViewModel(
            sessionService,
            currentClinicService.Object,
            currentEmployeeService.Object,
            navigationService.Object,
            appCenterAnalitics.Object);
    }

    internal CheckoutViewModelBuilder WithEmployee()
    {
        currentEmployeeService.Setup(x => x.Employee).Returns(new Models.Core.Employees.Employee());
        return this;
    }
    
    internal CheckoutViewModelBuilder WithCurrentClinic()
    {
        currentClinicService.Setup(x => x.CurrentClinic).Returns(new Models.Core.Clinics.ClinicResponse());
        return this;
    }
}
