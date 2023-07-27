namespace FreyaMobile.Features.Clients.Register;

public class RegisterViewModel : CoreViewModel
{
    private readonly IRegisterClientService userService;
    private readonly DialogService dialogService;

    private string weeksPregnancy;
    private bool arePregnant;

    public RegisterViewModel(IRegisterClientService userService, DialogService dialogService)
    {
        this.userService = userService;
        RegisterUser = new RegisterUserValidatable();
        arePregnant = true;
        weeksPregnancy = string.Empty;
        CreateCommand = new AsyncCommand(CreateAsync);
        this.dialogService = dialogService;
    }

    public RegisterUserValidatable RegisterUser { get; }

    public IAsyncCommand CreateCommand { get; set; }

    public string WeeksPregnancy 
    { 
        get => weeksPregnancy; 
        set => SetAndRaisePropertyChanged(ref weeksPregnancy, value); 
    }

    public bool ArePregnant
    {
        get => arePregnant;
        set
        {
            if (SetAndRaisePropertyChanged(ref arePregnant, value))
            {
                if (!arePregnant)
                {
                    WeeksPregnancy = string.Empty;
                }
            }
        }
    }

    public override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();
    }

    private async Task CreateAsync()
    {
        if (RegisterUser.HasError(out var error))
        {
            await dialogService.DisplayAlert(
                "Error",
                error,
                "Cerrar");
            return;
        }

        var result = await userService.RegisterUser(RegisterUser);

        if (result.IsSuccess)
        {
            await dialogService.PopAsync();
        }
    }
}
