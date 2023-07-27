namespace Freya.Features.Clients;

public class CheckClientViewModel : CoreViewModel
{
    private string email;
    private readonly INavigationService navigationService;
    private readonly IDialogService dialogService;
    private readonly ISessionService sessionService;
    private readonly ICheckClientService checkClientService;
    private readonly ITranslatorService translatorService;
    private readonly AppCenterAnalitics appCenterAnalitics;
    private Guid clinicId;

    public CheckClientViewModel(
       INavigationService navigationService,
       ICheckClientService checkClientService,
       IDialogService dialogService,
       ISessionService sessionService,
       ITranslatorService translatorService,
       AppCenterAnalitics appCenterAnalitics)
    {
        email = string.Empty;

        this.navigationService = navigationService;
        this.checkClientService = checkClientService;
        this.dialogService = dialogService;
        this.sessionService = sessionService;
        this.translatorService = translatorService;

        CheckClientCommand = new AsyncCommand(CheckClientCommandExecute, IsValidEmail);
        BackCommand = new AsyncCommand(BackCommandExecute);

        this.ShowBackButton = true;
        Title = translatorService.Translate("page_title_add_client");
        this.appCenterAnalitics = appCenterAnalitics;

        appCenterAnalitics.PageView(nameof(CheckClientViewModel));
    }

    private Task BackCommandExecute()
    {
        return navigationService.BackAsync();
    }

    public IAsyncCommand BackCommand { get; set; }

    public override Task OnAppearingAsync(object? parameter = null)
    {
        clinicId = sessionService.Get<Guid>(SESSION.KEY_CLINIC_ID_SELECTED);
        return base.OnAppearingAsync();
    }

    private bool IsValidEmail(object arg)
    {
        return EmailValidatorService.IsValidEmail(email);
    }

    public string Email
    {
        get => email;
        set
        {
            SetAndRaisePropertyChanged(ref email, value);
            CheckClientCommand.RaiseCanExecuteChanged();
        }
    }

    public AsyncCommand CheckClientCommand { get; set; }

    private async Task CheckClientCommandExecute()
    {
        appCenterAnalitics.Clicked("Check client if assigned");
        IsBusy = true;
        var result = await checkClientService.CheckNewClientState(Email, clinicId);
        IsBusy = false;

        if (result.IsSuccess)
        {
            await EvaluateResponse(result.Value);
        }
    }

    private async Task EvaluateResponse(NewClientState newClientState)
    {
        appCenterAnalitics.AddClient(newClientState.ToString());
        switch (newClientState)
        {
            case NewClientState.CLIENT_NOT_REGISTERED:
                await ClientNotRegister();
                break;
            case NewClientState.CLIENT_REGISTERED_AND_ASIGNED:
                await ClientRegisterAndAsigned();
                break;
            case NewClientState.CLIENT_REGISTERED_BUT_NOT_ASIGNED:
                await ClientRegisterButNotAsigned();
                break;
            default:
                break;
        }
    }

    private async Task ClientRegisterButNotAsigned()
    {
        await dialogService.ShowMessageYesNo(
                            translatorService.Translate("dialog_new_client_client_registered_title"),
                            translatorService.Translate("dialog_new_client_client_registered_body"),
                            new AsyncCommand(async () =>
                            {
                                appCenterAnalitics.Clicked("Check client if assigned - Assign client to clinic");
                                var result = await checkClientService.AssignClientToSelectedClinic(Email, clinicId);
                                if (result.IsSuccess)
                                {
                                    await dialogService.ShowMessage(
                                        translatorService.Translate("dialog_new_client_client_assigned_title"),
                                        translatorService.Translate("dialog_new_client_client_assigned_body"));
                                    await navigationService.BackAsync();
                                }
                            }));
    }

    private async Task ClientRegisterAndAsigned()
    {
        appCenterAnalitics.Clicked("Check client if assigned - Client registered and assigned");
        await dialogService.ShowMessage(
                translatorService.Translate("dialog_new_client_client_assigned_before_title"),
                translatorService.Translate("dialog_new_client_client_assigned_before_body"));
    }

    private async Task ClientNotRegister()
    {
        await dialogService.ShowMessageYesNo(
                            translatorService.Translate("dialog_new_client_client_exist_title"),
                            translatorService.Translate("dialog_new_client_client_exist_body"),
                            new AsyncCommand(async () =>
                            {
                                appCenterAnalitics.Clicked("Check client if assigned - Client not register");
                                sessionService.Save(SESSION.KEY_NEW_CLIENT_EMAIL_PRESELECTED, Email);
                                await navigationService.NavigateTo(typeof(CreateNewClientPage));
                            }));
    }
}
