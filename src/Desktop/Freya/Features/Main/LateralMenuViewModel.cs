namespace Freya.Features.Main;

public class LateralMenuViewModel : CoreViewModel
{
    private bool isMenuVisible;
    private bool canManageCompany;
    private Employee employee;
    private readonly INavigationService navigationService;
    private readonly ICurrentEmployeeService currentEmployeeService;
    private readonly IEmployeeRolService employeeRolService;
    private readonly ICurrentClinicService currentClinicService;
    private readonly IDialogService dialogService;
    private readonly ISessionService sessionService;
    private readonly ITranslatorService translatorService;
    private readonly AppCenterAnalitics appCenterAnalitics;

    public LateralMenuViewModel(
        INavigationService navigationService,
        IEmployeeRolService employeeRolService,
        ICurrentEmployeeService currentEmployeeService,
        IDialogService dialogService,
        ISessionService sessionService,
        ICurrentClinicService currentClinicService,
        ITranslatorService translatorService,
        AppCenterAnalitics appCenterAnalitics)
    {
        this.navigationService = navigationService;
        this.employeeRolService = employeeRolService;
        this.currentEmployeeService = currentEmployeeService;
        this.sessionService = sessionService;
        this.dialogService = dialogService;
        this.currentClinicService = currentClinicService;
        this.translatorService = translatorService;

        ShowMenuCommand = new AsyncCommand(ShowMenuCommandExecute);
        SeeProfileCommand = new AsyncCommand(SeeProfileCommandExecute);
        GoToClinicsCommand = new AsyncCommand(GoToClinicsCommandExecute);
        GoToClientsCommand = new AsyncCommand(GoToClientsCommandExecute);
        CloseSessionCommand = new AsyncCommand(CloseSessionCommandExecute);
        GoToSettingsCommand = new AsyncCommand(GoToSettingsCommandExecute);
        GoToClinicDetailCommand = new AsyncCommand(GoToClinicDetailCommandExecute);
        GoToFAQsCommand = new AsyncCommand(GoToFAQsCommandExecute);

        employee = new Employee();
        this.appCenterAnalitics = appCenterAnalitics;
    }

    public IAsyncCommand ShowMenuCommand { get; set; }

    public IAsyncCommand SeeProfileCommand { get; set; }

    public IAsyncCommand GoToClinicsCommand { get; set; }

    public IAsyncCommand GoToClientsCommand { get; set; }

    public IAsyncCommand CloseSessionCommand { get; set; }

    public IAsyncCommand GoToSettingsCommand { get; set; }

    public IAsyncCommand GoToClinicDetailCommand { get; set; }

    public IAsyncCommand GoToFAQsCommand { get; set; }

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

    public bool CanManageCompany
    {
        get
        {
            return canManageCompany;
        }

        set
        {
            SetAndRaisePropertyChanged(ref canManageCompany, value);
        }
    }

    public Employee Employee { get => employee; set => SetAndRaisePropertyChanged(ref employee, value); }

    public override async Task OnAppearingAsync(object? parameter = null)
    {
        Employee = currentEmployeeService.Employee ?? new Employee();
        CanManageCompany = employeeRolService.IsCompanyAdmin(employee.Rol);
        await base.OnAppearingAsync();
    }

    private Task ShowMenuCommandExecute()
    {
        IsMenuVisible = !IsMenuVisible;
        return Task.CompletedTask;
    }
    
    private async Task SeeProfileCommandExecute()
    {
        appCenterAnalitics.MenuClicked("Profile");
        IsMenuVisible = false;
        await navigationService.NavigateTo(typeof(ProfilePage));
    }

    private async Task GoToSettingsCommandExecute()
    {
        appCenterAnalitics.MenuClicked("Settings");
        IsMenuVisible = false;
        await navigationService.NavigateTo(typeof(SettingPage), clearStack: true);
    }
    
    private async Task GoToFAQsCommandExecute()
    {
        appCenterAnalitics.MenuClicked("Faqs");
        IsMenuVisible = false;
        await navigationService.NavigateTo(typeof(FaqPage), clearStack: true);
    }

    private async Task CloseSessionCommandExecute()
    {
        await dialogService.ShowMessageYesNo(
                    translatorService.Translate("dialog_menu_close_session_title"),
                    translatorService.Translate("dialog_menu_close_session_body"),
                    new AsyncCommand(async () =>
                    {
                        appCenterAnalitics.MenuClicked("Close session");
                        if (MainViewModel != null) 
                        {
                            await currentEmployeeService.SetEmployee(null);
                            IsMenuVisible = false;
                            MainViewModel.IsLogged = false;
                            await navigationService.NavigateTo(typeof(SignInPage), clearStack: true);
                        }
                    }));

    }

    private async Task GoToClientsCommandExecute()
    {
        appCenterAnalitics.MenuClicked("Clients");
        IsMenuVisible = false;
        await navigationService.NavigateTo(typeof(ClientsPage), clearStack: true);
    }

    private async Task GoToClinicsCommandExecute()
    {
        appCenterAnalitics.MenuClicked("Clinics");
        IsMenuVisible = false;
        await navigationService.NavigateTo(typeof(ClinicsPage), clearStack: true);
    }

    internal async Task GoBack()
    {
        await navigationService.BackAsync();
    }

    private async Task GoToClinicDetailCommandExecute()
    {
        appCenterAnalitics.MenuClicked("Clinic detail");
        IsMenuVisible = false;
        var clinicId = currentClinicService.CurrentClinic?.Id ?? System.Guid.Empty;
        sessionService.Save(SESSION.KEY_CLINIC_ID_SELECTED, clinicId);
        await navigationService.NavigateTo(typeof(ClinicDetailPage), clearStack: true);
    }
}

internal class LateralMenuViewModelForDesing : LateralMenuViewModel
{
    public LateralMenuViewModelForDesing()
        : base(null!, null!, null!, null!, null!, null!, null!, null!)
    {
    }
}
