namespace FreyaMobile.Features.Clients.ForgotPass;

public class ForgotPassViewModel : CorePopupViewModel
{
    private ForgotPassModelValidatable forgotPassModel;
    private readonly IForgotPassService userService;
    private readonly DialogService dialogService;

    public ForgotPassViewModel(
        IForgotPassService userService,
        DialogService dialogService)
    {
        this.userService = userService;

        this.ForgotPassCommand = new AsyncCommand(this.ForgotPassAsync);

        this.forgotPassModel = new ForgotPassModelValidatable();
        this.dialogService = dialogService;
    }

    public IAsyncCommand ForgotPassCommand { get; set; }

    public ForgotPassModelValidatable ForgotPassModel { get => forgotPassModel; set => SetAndRaisePropertyChanged(ref forgotPassModel, value); }

    public override Task OnAppearingAsync()
    {
        //if (parameter is string previousEmail)
        //{
        //    this.forgotPassModel.SetEmail(previousEmail);
        //}
        return base.OnAppearingAsync();
    }

    private async Task ForgotPassAsync()
    {
        if (forgotPassModel.HasError(out var error))
        {
            //TODO Creo que en ios no se puede mostrar el dialogo con otro mostrado
            await dialogService.DisplayAlert(
                "Error",
                error,
                "Cerrar");
            return;
        }

        var result = await userService.ForgorPassAsync(forgotPassModel.Email.Value);

        if (result.IsSuccess)
        {

            await this.ClosePopupAsync();
            await dialogService.DisplayAlert("Revise su correo", string.Empty, "Cerrar");
        }
    }

}
