namespace Freya.Desktop.Core.Tests.Features.Clients;

internal class EditClientViewModelBuilder
{
    private readonly Mock<IDialogService> dialogService;
    public readonly ISessionService SessionService;
    private readonly Mock<EditClientService> editClientService;
    private readonly Mock<INavigationService> navigationService;
    private readonly Mock<ITranslatorService> translatorService;
    private readonly Mock<AppCenterAnalitics> appCenterAnalitics;

    public EditClientViewModelBuilder()
    {
        dialogService = new Mock<IDialogService>();
        SessionService = new SessionService();

        var refitService = new Mock<IRefitService>();
        editClientService = new Mock<EditClientService>(null, refitService.Object, null, null, null);
        navigationService = new Mock<INavigationService>();
        translatorService = new Mock<ITranslatorService>();
        appCenterAnalitics = new Mock<AppCenterAnalitics>();
    }

    internal EditClientViewModel Build()
    {
        return new EditClientViewModel(
            SessionService,
            navigationService.Object,
            dialogService.Object,
            editClientService.Object,
            translatorService.Object,
            appCenterAnalitics.Object);
    }
}
