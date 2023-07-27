namespace Freya.Tests.Features.Clinics;

internal class ClinicsViewModelBuilder
{
    public readonly ISessionService sessionService;
    public readonly Mock<IServiceProvider> serviceProvider;
    public readonly Mock<INavigationService> navigationService;
    private readonly Mock<ITranslatorService> translatorService;
    private readonly Mock<IClinicsService> clinicsService;
    private readonly Mock<AppCenterAnalitics> appCenterAnalitics;

    public ClinicsViewModelBuilder()
    {
        this.serviceProvider = new Mock<IServiceProvider>();
        this.navigationService = new Mock<INavigationService>();
        this.translatorService = new Mock<ITranslatorService>();
        this.sessionService = new SessionService();
        this.clinicsService = new Mock<IClinicsService>();
        this.appCenterAnalitics = new Mock<AppCenterAnalitics>();
    }

    internal ClinicsViewModel Build()
    {
        return new ClinicsViewModel(
            clinicsService.Object,
            navigationService.Object,
            serviceProvider.Object,
            translatorService.Object,
            appCenterAnalitics.Object);
    }
}
