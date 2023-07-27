using Models.Core.AppManagers;

namespace FreyaManager.Features.Main;

public class LateralMenuViewModel : CoreViewModel
{
    private bool isMenuVisible;
    private AppManager employee;
    private readonly INavigationService navigationService;
    private readonly ICurrentAppManagerService currentEmployeeService;
    private readonly IDialogService dialogService;

    public LateralMenuViewModel(
        INavigationService navigationService,
        ICurrentAppManagerService currentEmployeeService,
        IDialogService dialogService)
    {
        this.navigationService = navigationService;
        this.currentEmployeeService = currentEmployeeService;
        this.dialogService = dialogService;

        ShowMenuCommand = new AsyncCommand(ShowMenuCommandExecute);
        GoToSubscriptionsCommand = new AsyncCommand(GoToSubscriptionsCommandExecute);
        GoToCompaniesCommand = new AsyncCommand(GoToCompaniesCommandExecute);
        CloseSessionCommand = new AsyncCommand(CloseSessionCommandExecute);
        GoToFaqCommand = new AsyncCommand(GoToFaqCommandExecute);
        GoToSettingsCommand = new AsyncCommand(GoToSettingsCommandExecute);

        employee = new AppManager();
    }

    public IAsyncCommand GoToSettingsCommand { get; set; }

    public IAsyncCommand ShowMenuCommand { get; set; }

    public IAsyncCommand GoToSubscriptionsCommand { get; set; }

    public IAsyncCommand GoToCompaniesCommand { get; set; }

    public IAsyncCommand GoToFaqCommand { get; set; }

    public IAsyncCommand CloseSessionCommand { get; set; }

    public MainViewModel? MainViewModel { get; set; }

    public bool IsMenuVisible
    {
        get
        {
            return isMenuVisible;
        }

        set
        {
            SetAndRaisePropertyChanged(ref isMenuVisible, value);
        }
    }

    public AppManager Employee { get => employee; set => SetAndRaisePropertyChanged(ref employee, value); }

    public override async Task OnAppearingAsync(object? parameter = null)
    {
        Employee = currentEmployeeService.Employee ?? new AppManager();

        await base.OnAppearingAsync();
    }

    private Task ShowMenuCommandExecute()
    {
        IsMenuVisible = !IsMenuVisible;
        return Task.CompletedTask;
    }

    private async Task CloseSessionCommandExecute()
    {
        await dialogService.ShowMessageYesNo(
                    "Cerrar sesión",
                    "¿Estas seguro de que quieres salir?",
                    new AsyncCommand(async () =>
                    {
                        await currentEmployeeService.Set(null);
                        IsMenuVisible = false;
                        MainViewModel!.IsLogged = false;
                        await navigationService.NavigateTo(typeof(SignInPage), clearStack: true);
                    }));

    }

    private async Task GoToCompaniesCommandExecute()
    {
        IsMenuVisible = false;
        await navigationService.NavigateTo(typeof(CompaniesPage), clearStack: true);
    }
    
    private async Task GoToFaqCommandExecute()
    {
        IsMenuVisible = false;
        await navigationService.NavigateTo(typeof(FaqPage), clearStack: true);
    }
    
    private async Task GoToSettingsCommandExecute()
    {
        IsMenuVisible = false;
        await navigationService.NavigateTo(typeof(SettingPage), clearStack: true);
    }

    private async Task GoToSubscriptionsCommandExecute()
    {
        IsMenuVisible = false;
        await navigationService.NavigateTo(typeof(SubscriptionsPage), clearStack: true);
    }

    internal async Task GoBack()
    {
        await navigationService.BackAsync();
    }
}

internal class LateralMenuViewModelForDesing : LateralMenuViewModel
{
    public LateralMenuViewModelForDesing()
        : base(null!, null!, null!)
    {
    }
}
