namespace FreyaMobile.Features.Clients;

public class ClientViewModel : CoreViewModel
{
    private bool isLogged;
    private readonly ICurrentUserService currentUserService;

    public ClientViewModel(
        UserLoggedViewModel userLoggedViewModel,
        UserNotLoggedViewModel userNotLoggedViewModel,
        ICurrentUserService currentUserService)
    {
        this.UserLoggedViewModel = userLoggedViewModel;
        this.UserNotLoggedViewModel = userNotLoggedViewModel;

        this.UserNotLoggedViewModel.Parent = this;
        this.UserLoggedViewModel.Parent = this;

        IsLogged = false;
        this.currentUserService = currentUserService;
    }

    public UserLoggedViewModel UserLoggedViewModel { get; private set; }

    public UserNotLoggedViewModel UserNotLoggedViewModel { get; private set; }

    public bool IsLogged
    {
        get => isLogged;
        set => SetAndRaisePropertyChanged(ref isLogged, value);
    }

    public override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();
        await ReloadAsync();
    }

    public override Task OnDisappearingAsync()
    {
        return base.OnDisappearingAsync();
    }

    public async Task ReloadAsync()
    {
        IsLogged = await currentUserService.IsLoggedAsync();
        if (isLogged)
        {
            await this.UserLoggedViewModel.OnAppearingAsync();
        }
        else
        {
            await this.UserNotLoggedViewModel.OnAppearingAsync();
        }
    }
}
