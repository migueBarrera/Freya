using MiBebeClient.Features.Clients;

namespace FreyaMobile.Features.Clients;

public class UserNotLoggedViewModel : CoreViewModel
{
    private readonly IUserService userService;
    private readonly IServiceProvider serviceProvider;
    private readonly DialogService dialogService;

    public UserNotLoggedViewModel(
        IUserService userService,
        IServiceProvider serviceProvider,
        DialogService dialogService)
    {
        this.userService = userService;
        this.serviceProvider = serviceProvider;

        this.EnterCommand = new AsyncCommand(this.EnterAsync);
        this.RegisterCommand = new AsyncCommand(this.RegisterAsync);
        this.ForgotPassCommand = new AsyncCommand(this.ForgotPassAsync);

        SignInUser = new ClientSignInValidatable();
        this.dialogService = dialogService;

        //#if DEBUG
        //        SignInUser.Email.Value = "juan@yopmail.com";
        //        SignInUser.Pass.Value = "123456";
        //#endif
    }

    public ClientSignInValidatable SignInUser { get; }

    public ClientViewModel? Parent { get; set; }

    public IAsyncCommand EnterCommand { get; set; }

    public IAsyncCommand RegisterCommand { get; set; }

    public IAsyncCommand ForgotPassCommand { get; set; }

    public override Task OnAppearingAsync()
    {
        return base.OnAppearingAsync();
    }

    public override Task OnDisappearingAsync()
    {
        return base.OnDisappearingAsync();
    }

    private async Task EnterAsync()
    {
        if (SignInUser.HasError(out var error))
        {
            await dialogService.DisplayAlert(
                "Error",
                error,
                "Cerrar");
            return;
        }

        var result = await userService.LogInUser(SignInUser.Email.Value, SignInUser.Pass.Value);

        if (result.IsSuccess)
        {
            SignInUser.Email.Value = string.Empty;
            SignInUser.Pass.Value = string.Empty;
            await this.Parent!.ReloadAsync();
        }
    }

    private async Task RegisterAsync()
    {
        await Shell.Current.GoToAsync(nameof(RegisterPage));
    }

    private async Task ForgotPassAsync()
    {
        var popup = serviceProvider.GetService<ForgotPassPage>();
        await Application.Current!.MainPage!.ShowPopupAsync(popup!);
    }
}
