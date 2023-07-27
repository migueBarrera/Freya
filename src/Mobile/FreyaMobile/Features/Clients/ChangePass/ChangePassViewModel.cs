namespace FreyaMobile.Features.Clients.ChangePass;

public class ChangePassViewModel : CorePopupViewModel
{
    private readonly IChangePassService userService;
    private readonly DialogService dialogService;


    public ChangePassViewModel(
        IChangePassService userService,
        DialogService dialogService)
    {
        this.userService = userService;
        this.dialogService = dialogService;

        this.ChangePassValidatable = new ChangePassValidatable();

        this.ChangePassCommand = new AsyncCommand<Popup>(this.ChangePassAsync);
    }

    public ChangePassValidatable ChangePassValidatable { get; }

    public AsyncCommand<Popup> ChangePassCommand { get; set; }

    private async Task ChangePassAsync(Popup popup)
    {
        if (ChangePassValidatable.HasError(out var error))
        {
            return;
        }

        var result = await userService.ChangePassAsync(ChangePassValidatable.NewPass.Value, ChangePassValidatable.ActualPass.Value);

        if (result.IsSuccess)
        {
            await this.ClosePopupAsync(popup);
            await dialogService.DisplayAlert("Contraseña cambiada", string.Empty, "Cerrar");
        }
    }
}
