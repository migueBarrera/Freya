using Freya.Desktop.Core.Features.Clinics;

namespace Freya.Features.Clinics;

public class EditClinicViewModel : CoreViewModel
{
	private readonly ISessionService sessionService;
	private readonly INavigationService navigationService;
	private readonly IDialogService dialogService;
    private readonly ITranslatorService translatorService;
    private readonly IEditClinicService editClinicService;
    private readonly AppCenterAnalitics appCenterAnalitics;
    private NewClinicModel newClinicModel;

    public EditClinicViewModel(
        ISessionService sessionService,
        INavigationService navigationService,
        IEditClinicService editClinicService,
        IDialogService dialogService,
        ITranslatorService translatorService,
        AppCenterAnalitics appCenterAnalitics)
    {
        ShowBackButton = true;
        Title = translatorService.Translate("page_title_edit_clinic");
        this.sessionService = sessionService;
        SaveChangesEditClinicCommand = new AsyncCommand(SaveChangesEditClinicCommandExecute);
        BackCommand = new AsyncCommand(BackCommandExecute);
        newClinicModel = new NewClinicModel();
        this.navigationService = navigationService;
        this.editClinicService = editClinicService;
        this.dialogService = dialogService;
        this.translatorService = translatorService;
        this.appCenterAnalitics = appCenterAnalitics;

        appCenterAnalitics.PageView(nameof(EditClinicViewModel));
    }

    public override Task OnAppearingAsync(object? parameter = null)
	{
        ClinicDetailResponse responseClinic = sessionService.Get<ClinicDetailResponse>(SESSION.KEY_CLINIC_SELECTED);
        NewClinicModel = new NewClinicModel(responseClinic);
		return base.OnAppearingAsync(parameter);
	}

    private Task BackCommandExecute()
    {
        return navigationService.BackAsync();
    }

    public IAsyncCommand BackCommand { get; set; }

    public AsyncCommand SaveChangesEditClinicCommand { get; set; }

    public NewClinicModel NewClinicModel
    {
        get => newClinicModel;
        set
        {
            SetAndRaisePropertyChanged(ref newClinicModel, value);
        }
    }

    private async Task SaveChangesEditClinicCommandExecute()
    {
        if (string.IsNullOrWhiteSpace(NewClinicModel.Name)
           || string.IsNullOrWhiteSpace(NewClinicModel.NIF)
           || string.IsNullOrWhiteSpace(NewClinicModel.Adress))
        {
            await dialogService.ShowMessage(translatorService.Translate("error"),
                translatorService.Translate("check_fields"));
            return;
        }

        appCenterAnalitics.Clicked("Save clinic data");
        IsBusy = true;

        var result = await editClinicService.EditClinic(NewClinicModel.ToEditRequest());

        IsBusy = false;
        if (result)
        {
            await navigationService.BackAsync();
        }
    }
}
