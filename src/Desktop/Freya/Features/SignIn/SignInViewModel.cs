namespace Freya.Features.SignIn;

public class SignInViewModel : CoreViewModel
{
    private readonly INavigationService navigationService;
    private readonly ISignInService signInService;
    private readonly IDialogService dialogService;
    private readonly ITranslatorService translatorService;
    private readonly IAppUpdaterManagerService appUpdaterService;
    private readonly AppCenterAnalitics appCenterService;
    private string email = string.Empty;
    private string pass = string.Empty;

    public SignInViewModel(
                INavigationService navigationService,
                ISignInService signInService,
                IDialogService dialogService,
                IAppUpdaterManagerService appUpdaterService,
                ITranslatorService translatorService,
                AppCenterAnalitics appCenterService)
    {
        this.navigationService = navigationService;
        this.signInService = signInService;
        this.dialogService = dialogService;
        this.appUpdaterService = appUpdaterService;
        this.translatorService = translatorService;
        this.appCenterService = appCenterService;
        this.appCenterService = appCenterService;
        EnterCommand = new AsyncCommand(EnterCommandExecute);
        RecoverPassCommand = new AsyncCommand(RecoverPassCommandExecute);

#if DEBUG
        email = "mbmdevelop@gmail.com";
        pass = "123456";
#endif

        appCenterService.PageView(nameof(SignInViewModel));
    }

    public IAsyncCommand EnterCommand { get; set; }

    public IAsyncCommand RecoverPassCommand { get; set; }

    public string Email { get => email; set => SetAndRaisePropertyChanged(ref email, value); }

    public string Pass { get => pass; set => SetAndRaisePropertyChanged(ref pass, value); }

    public override async Task OnAppearingAsync(object? parameter = null)
    {
        await base.OnAppearingAsync(parameter);
        await appUpdaterService.CheckAndTryUpdate(showErrorDialog: false);
    }

    private async Task EnterCommandExecute()
    {
        if (string.IsNullOrWhiteSpace(Email) 
            || string.IsNullOrWhiteSpace(Pass) 
            || !EmailValidatorService.IsValidEmail(Email))
        {
            await dialogService.ShowMessage(
                translatorService.Translate("error"),
                translatorService.Translate("check_fields"));
            return;
        }

        appCenterService.Clicked("Sign in button");

        IsBusy = true;
        var result = await signInService.SignInAsync(Email, Pass);

        if (result)
        {
            if (Parent is MainViewModel viewModel)
            {
                viewModel.IsLogged = true;
                await viewModel.LoadItems();
            }
            await navigationService.NavigateTo(typeof(ClientsPage), clearStack: true);
        }

        IsBusy = false;
    }
    
    private async Task RecoverPassCommandExecute()
    {
        appCenterService.Clicked("Forgot pass button");
        if (!EmailValidatorService.IsValidEmail(Email))
        {
            await dialogService.ShowMessage(
                translatorService.Translate("error"),
                translatorService.Translate("check_fields"));
            return;
        }

        await dialogService.ShowMessageYesNo(
            translatorService.Translate("dialog_sign_in_recover_pass_question_title"),
            translatorService.Translate("dialog_sign_in_recover_pass_question_body"),
            new AsyncCommand(UserWantResetPass));
    }

    private async Task UserWantResetPass()
    {
        IsBusy = true;
        var result = await signInService.RecoverPass(Email);

        IsBusy = false;
        if (result)
        {
            await dialogService.ShowMessage(
                translatorService.Translate("dialog_sign_in_recover_pass_success_title"),
                translatorService.Translate("dialog_sign_in_recover_pass_success_body"));
        }
    }
}
