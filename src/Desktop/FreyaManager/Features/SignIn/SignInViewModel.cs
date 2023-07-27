namespace FreyaManager.Features.SignIn;

public class SignInViewModel : CoreViewModel
{
    private readonly INavigationService navigationService;
    private readonly ISignInService signInService;
    private readonly IAppUpdaterManagerService appUpdaterService;
    private string email;
    private string pass;

    public SignInViewModel(
        INavigationService navigationService,
        ISignInService signInService,
        IAppUpdaterManagerService appUpdaterService)
    {
        this.navigationService = navigationService;
        this.signInService = signInService;
        this.appUpdaterService = appUpdaterService;
        EnterCommand = new AsyncCommand(EnterCommandExecute);

        email = string.Empty;
        pass = string.Empty;
#if DEBUG
        email = "mbmdevelop@gmail.com";
        pass = "123456";
#endif
    }

    public IAsyncCommand EnterCommand { get; set; }

    public string Email { get => email; set => SetAndRaisePropertyChanged(ref email, value); }

    public string Pass { get => pass; set => SetAndRaisePropertyChanged(ref pass, value); }

    public override async Task OnAppearingAsync(object? parameter = null)
    {
        await base.OnAppearingAsync(parameter);
        await appUpdaterService.CheckAndTryUpdate(showErrorDialog: false);
    }

    private async Task EnterCommandExecute()
    {
        IsBusy = true;
        var result = await signInService.SignInAsync(Email, Pass);
        IsBusy = false;
        if (result)
        {
            ((MainViewModel)Parent!).IsLogged = true;
            await ((MainViewModel)Parent!).LoadItems();
            await navigationService.NavigateTo(typeof(CompaniesPage), true);
        }
    }
}
