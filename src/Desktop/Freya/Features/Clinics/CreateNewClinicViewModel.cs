using Freya.Desktop.Core.Features.Clinics;

namespace Freya.Features.Clinics;

public class CreateNewClinicViewModel : CoreViewModel
{
    private readonly ICreateNewClinicService createNewClinicService;
    private readonly INavigationService navigationService;
    private readonly IDialogService dialogService;
    private readonly ICurrentEmployeeService currentEmployeeService;
    private readonly ITranslatorService translatorService;
    private readonly AppCenterAnalitics appCenterAnalitics;
    private NewClinicModel newClinicModel;
    private Guid companyId;

    public CreateNewClinicViewModel(
        ICreateNewClinicService createNewClinicService,
        INavigationService navigationService,
        ICurrentEmployeeService currentEmployeeService,
        IDialogService dialogService,
        ITranslatorService translatorService,
        AppCenterAnalitics appCenterAnalitics)
    {
        this.createNewClinicService = createNewClinicService;
        this.navigationService = navigationService;
        this.currentEmployeeService = currentEmployeeService;
        this.dialogService = dialogService;
        this.translatorService = translatorService;
        this.appCenterAnalitics = appCenterAnalitics;

        Title = translatorService.Translate("page_title_new_clinic");
        ShowBackButton = true;
        newClinicModel = new NewClinicModel();
        CreateClinicCommand = new AsyncCommand(CreateClinicCommandExecute);
        BackCommand = new AsyncCommand(BackCommandExecute);

    }

    public override Task OnAppearingAsync(object? parameter = null)
    {
        companyId = currentEmployeeService.Employee!.CompanyId;
        return base.OnAppearingAsync(parameter);
    }

    private Task BackCommandExecute()
    {
        return navigationService.BackAsync();
    }

    public IAsyncCommand BackCommand { get; set; }

    public AsyncCommand CreateClinicCommand { get; set; }

    public NewClinicModel NewClinicModel
    {
        get => newClinicModel;
        set
        {
            SetAndRaisePropertyChanged(ref newClinicModel, value);
        }
    }

    private async Task CreateClinicCommandExecute()
    {
        if (string.IsNullOrWhiteSpace(NewClinicModel.Name) 
            || string.IsNullOrWhiteSpace(NewClinicModel.NIF)
            || string.IsNullOrWhiteSpace(NewClinicModel.Adress))
        {
            await dialogService.ShowMessage(translatorService.Translate("error"),
                translatorService.Translate("check_fields"));
            return;
        }

        IsBusy = true;

        var result = await createNewClinicService.CreateClinic(NewClinicModel.ToRequest(companyId));

        IsBusy = false;
        if (result != null)
        {
            appCenterAnalitics.ClinicCreated(companyId);
            (Parent as MainViewModel)?.HeaderControlViewModel.SetClinic(result.Id, result.Name);
            await navigationService.BackAsync();
        }
    }
}
