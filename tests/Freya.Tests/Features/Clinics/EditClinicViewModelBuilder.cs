namespace Freya.Tests.Features.Clinics;

internal class EditClinicViewModelBuilder
{
    public readonly ISessionService sessionService;
    public readonly Mock<IServiceProvider> serviceProvider;
    public readonly Mock<INavigationService> navigationService;
    private readonly Mock<ITranslatorService> translatorService;
    private readonly Mock<IEditClinicService> editClinicService;
    private readonly Mock<IDialogService> dialogService;
    private readonly Mock<AppCenterAnalitics> appCenterAnalitics;

    public EditClinicViewModelBuilder()
    {
        this.serviceProvider = new Mock<IServiceProvider>();
        this.navigationService = new Mock<INavigationService>();
        this.translatorService = new Mock<ITranslatorService>();
        this.sessionService = new SessionService();
        this.editClinicService = new Mock<IEditClinicService>();
        this.dialogService = new Mock<IDialogService>();
        this.appCenterAnalitics = new Mock<AppCenterAnalitics>();
    }

    internal EditClinicViewModel Build()
    {
        return new EditClinicViewModel(
            sessionService,
            navigationService.Object,
            editClinicService.Object,
            dialogService.Object,
            translatorService.Object,
            appCenterAnalitics.Object);
    }
}
