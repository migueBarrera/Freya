namespace FreyaMobile.Features.Clients;

public class EditClientViewModel : CoreViewModel
{
    private readonly ICurrentUserService currentUserService;
    private readonly IEditClientService userService;
    private readonly DialogService dialogService;
    private Client? user;
    private string weeksPregnancy;
    private bool arePregnant;

    public EditClientViewModel(
        ICurrentUserService currentUserService,
        IEditClientService userService,
        DialogService dialogService)
    {
        this.currentUserService = currentUserService;
        this.userService = userService;
        this.dialogService = dialogService;
        this.arePregnant = true;
        this.weeksPregnancy = string.Empty;
        this.SaveUserCommand = new AsyncCommand(SaveUserAsync);

        this.UpdateUser = new UpdateUserValidatable();
    }

    public IAsyncCommand SaveUserCommand { get; set; }

    public UpdateUserValidatable UpdateUser { get; }

    public string WeeksPregnancy { get => weeksPregnancy; set => SetAndRaisePropertyChanged(ref weeksPregnancy, value); }

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
        user = await currentUserService.GetCurrentUserAsync();
        UpdateUser.Name.Value = user!.Name;
        UpdateUser.Phone.Value = user!.Phone;
        UpdateUser.LastName.Value = user!.LastName;
    }

    private async Task SaveUserAsync()
    {
        if (UpdateUser.HasError(out var error))
        {
            await dialogService.DisplayAlert(
                "Error",
                error,
                "Cerrar");
            return;
        }

        var result = await userService.UpdateUser(UpdateUser);

        if (result.IsSuccess)
        {
            await dialogService.DisplayAlert(
                "Datos actualizados", 
                "",
                "Cerrar");
            await dialogService.PopAsync();
        }
    }
}
