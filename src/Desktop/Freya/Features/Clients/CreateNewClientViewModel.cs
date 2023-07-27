using AppCenterServices;

namespace Freya.Features.Clients;

public class CreateNewClientViewModel : CoreViewModel
{
    private readonly ICreateClientService createClientService;
    private readonly IDialogService dialogService;
    private readonly ISessionService sessionService;
    private readonly INavigationService navigationService;
    private readonly ITranslatorService translatorService;
    private readonly AppCenterAnalitics appCenterAnalitics;
    private CreateClientModel clientModel;
    private bool hasEmailPreseleted;

    public CreateNewClientViewModel(
    IDialogService dialogService,
    ISessionService sessionService,
    ICreateClientService createClientService,
    INavigationService navigationService,
    ITranslatorService translatorService,
    AppCenterAnalitics appCenterAnalitics)
    {
        Title = translatorService.Translate("page_title_new_client");
        ShowBackButton = true;
        clientModel = new CreateClientModel();
        CreateEmployeeCommand = new AsyncCommand(CreateEmployeeCommandExecute);
        BackCommand = new AsyncCommand(BackCommandExecute);
        this.dialogService = dialogService;
        this.sessionService = sessionService;
        this.createClientService = createClientService;
        this.navigationService = navigationService;
        this.translatorService = translatorService;
        this.appCenterAnalitics = appCenterAnalitics;

        appCenterAnalitics.PageView(nameof(CreateNewClientViewModel));
    }

    private Task BackCommandExecute()
    {
        return navigationService.BackAsync();
    }

    public IAsyncCommand BackCommand { get; set; }

    public IAsyncCommand CreateEmployeeCommand { get; set; }

    public CreateClientModel ClientModel { get => clientModel; set => SetAndRaisePropertyChanged(ref clientModel, value); }

    public bool HasEmailPreseleted { get => hasEmailPreseleted; set => SetAndRaisePropertyChanged(ref hasEmailPreseleted, value); }

    public override async Task OnAppearingAsync(object? parameter = null)
    {
        await base.OnAppearingAsync();

        var emialPreselected = sessionService.Get<string>(SESSION.KEY_NEW_CLIENT_EMAIL_PRESELECTED);
        if (!string.IsNullOrWhiteSpace(emialPreselected))
        {
            ClientModel.EmployeeEmail = emialPreselected;
            HasEmailPreseleted = true;
        }
    }

    private async Task CreateEmployeeCommandExecute()
    {
        if (!ClientModel.Validate())
        {
            await dialogService.ShowMessage(
                translatorService.Translate("error"),
                translatorService.Translate("check_fields"));
            return;
        }

        appCenterAnalitics.Clicked("Create client");
        var clinicId = sessionService.Get<Guid>(SESSION.KEY_CLINIC_ID_SELECTED);
        IsBusy = true;
        var result = await createClientService.CreateClient(
            new Client()
            {
                Email = ClientModel.EmployeeEmail,
                LastName = ClientModel.EmployeeLastName,
                Name = ClientModel.EmployeeName,
                Phone = ClientModel.EmployeePhone.Replace(" ", string.Empty),
            },
            clinicId);
        IsBusy = false;

        if (result)
        {
            await dialogService.ShowMessage(
                translatorService.Translate("dialog_new_client_success_title"),
                translatorService.Translate("dialog_new_client_success_body"));
            await navigationService.BackAsync();
            await navigationService.BackAsync();
        }
    }
}
