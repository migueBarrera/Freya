using Freya.Desktop.Core.Features.Clients.Models;
using Freya.Desktop.Core.Features.Clients.Services;
using Freya.Desktop.Core.Services;
using Models.Core.Clients;
using MvvmHelpers.Commands;
using MvvmHelpers.Interfaces;

namespace Freya.Desktop.Core.Features.Clients;

public class EditClientViewModel : CoreViewModel
{
    private readonly ISessionService sessionService;
    private readonly INavigationService navigationService;
    private readonly IDialogService dialogService;
    private readonly ITranslatorService translatorService;
    private readonly AppCenterAnalitics appCenterAnalitics;
    private readonly EditClientService editClientService;
    private EditClientModel editClientModel;

    public EditClientViewModel(
        ISessionService sessionService,
        INavigationService navigationService,
        IDialogService dialogService,
        EditClientService editClientService,
        ITranslatorService translatorService,
        AppCenterAnalitics appCenterAnalitics)
    {
        this.sessionService = sessionService;
        this.navigationService = navigationService;
        this.dialogService = dialogService;
        this.editClientService = editClientService;
        this.translatorService = translatorService;

        ShowBackButton = true;
        Title = translatorService.Translate("page_title_edit_client");
        editClientModel = new EditClientModel();

        BackCommand = new AsyncCommand(BackCommandExecute);
        SaveChangesEditClientCommand = new AsyncCommand(SaveChangesEditClientCommandExecute);
        this.appCenterAnalitics = appCenterAnalitics;

        appCenterAnalitics.PageView(nameof(EditClientViewModel));
    }

    public IAsyncCommand SaveChangesEditClientCommand { get; set; }

    public IAsyncCommand BackCommand { get; set; }

    public override async Task OnAppearingAsync(object? parameter = null)
    {
        await base.OnAppearingAsync(parameter);

        var client = sessionService.Get<ClientDetailResponse>(SESSION.KEY_CLIENT_DETAIL_SELECTED);

        EditClientModel.Id = client.Id;
        EditClientModel.ClientEmail = client.Email;
        EditClientModel.ClientPhone = client.Phone;
        EditClientModel.ClientLastName = client.LastName;
        EditClientModel.ClientName = client.Name;
    }


    public EditClientModel EditClientModel { get => editClientModel; set => SetAndRaisePropertyChanged(ref editClientModel, value); }

    private Task BackCommandExecute()
    {
        return navigationService.BackAsync();
    }

    private async Task SaveChangesEditClientCommandExecute()
    {
        if (string.IsNullOrWhiteSpace(EditClientModel.ClientName)
           || string.IsNullOrWhiteSpace(EditClientModel.ClientEmail)
           || !EmailValidatorService.IsValidEmail(EditClientModel.ClientEmail))
        {
            await dialogService.ShowMessage(
                translatorService.Translate("error"),
                translatorService.Translate("check_fields"));
            return;
        }
        IsBusy = true;
        appCenterAnalitics.Clicked("Save client data");
        var result = await editClientService.EditClient(EditClientModel.Id, EditClientModel.ToEditRequest());

        IsBusy = false;
        if (result)
        {
            await navigationService.BackAsync();
        }
    }
}
