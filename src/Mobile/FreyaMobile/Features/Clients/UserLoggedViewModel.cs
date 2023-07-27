namespace FreyaMobile.Features.Clients;

public class UserLoggedViewModel : CoreViewModel
{
    private readonly ICurrentUserService currentUserService;
    private readonly DialogService dialogService;
    private readonly IUserService userService;
    private readonly IServiceProvider serviceProvider;
    private Client? user;
    private Clinic? clinicSelected;

    public UserLoggedViewModel(
       ICurrentUserService currentUserService,
       DialogService dialogService,
       IUserService userService,
       IServiceProvider serviceProvider)
    {
        this.currentUserService = currentUserService;
        this.userService = userService;
        this.dialogService = dialogService;

        this.CloseSessionCommand = new AsyncCommand(this.CloseSessionAsync);
        this.ChangePassCommand = new AsyncCommand(this.ChangePassAsync);
        this.EditUserCommand = new AsyncCommand(this.EditUserAsync);
        user = new Client();
        this.serviceProvider = serviceProvider;
    }

    public IAsyncCommand CloseSessionCommand { get; set; }

    public IAsyncCommand ChangePassCommand { get; set; }

    public IAsyncCommand EditUserCommand { get; set; }

    public ClientViewModel? Parent { get; set; }

    public Client? User
    {
        get => user;
        set => SetAndRaisePropertyChanged(ref user, value);
    }

    public Clinic? ClinicSelected
    {
        get => clinicSelected;
        set
        {
            SetAndRaisePropertyChanged(ref clinicSelected, value);
            if (value != null)
            {
                currentUserService.SetClinicSelectedAsync(value).Wait();
            }
        }
    }

    public override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();
        User = await currentUserService.GetCurrentUserAsync();
        var currentClinic = currentUserService.GetCurrentClinic();
        if (currentClinic != null)
        {
            ClinicSelected = User!.Clinics.First(x => x.Id == currentClinic.Id);
        }
        else
        {
            ClinicSelected = User!.Clinics.FirstOrDefault();
        }
    }

    private async Task CloseSessionAsync()
    {
        var popup = serviceProvider.GetService<YesNoDialog>();
        if(popup == null)
        {
            return;
        }

        popup.Title = "Cerrar sesión";
        popup.Body = "¿Estas seguro que deseas salir?";
        var wantCloseSession = await App.Current?.MainPage?.ShowPopupAsync(popup!)!;

        if (wantCloseSession is true)
        {
            await currentUserService.SetUserAsync(null);
            await currentUserService.SetClinicSelectedAsync(null);
            this.Parent!.IsLogged = false;
        }
    }

    private Task ChangePassAsync()
    {
        var popup = serviceProvider.GetService<ChangePassPage>();
        App.Current?.MainPage?.ShowPopup(popup!);

        return Task.CompletedTask;
    }

    private async Task EditUserAsync()
    {
        await Shell.Current.GoToAsync(nameof(EditClientPage));
    }
}
