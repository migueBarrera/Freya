using Freya.Desktop.Updater;
using Freya.Features.SignIn;

namespace Freya.Tests.Features.SignIn;

internal class SignInViewModelBuilder
{
    public readonly Mock<ISignInService> signInService;
    public readonly Mock<INavigationService> navigationService;
    private readonly Mock<IDialogService> dialogService;
    private readonly Mock<ITranslatorService> translatorService;
    private readonly Mock<IAppUpdaterManagerService> appUpdaterManagerService;
    private readonly Mock<AppCenterAnalitics> appCenterAnalitics;

    public SignInViewModelBuilder()
    {
        signInService = new Mock<ISignInService>();
        dialogService = new Mock<IDialogService>();
        navigationService = new Mock<INavigationService>();
        translatorService = new Mock<ITranslatorService>();
        appUpdaterManagerService = new Mock<IAppUpdaterManagerService>();
        appCenterAnalitics = new Mock<AppCenterAnalitics>();
    }

    internal SignInViewModel Build()
    {
        return new SignInViewModel(
            navigationService.Object,
            signInService.Object,
            dialogService.Object,
            appUpdaterManagerService.Object,
            translatorService.Object,
            appCenterAnalitics.Object);
    }

    internal SignInViewModelBuilder SignInSuccess(string email, string pass)
    {
        signInService.Setup(x => x.SignInAsync(email, pass)).Returns(Task.FromResult(true));
        return this;
    }
}
