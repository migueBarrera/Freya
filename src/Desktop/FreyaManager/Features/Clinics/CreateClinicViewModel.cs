using Models.Core.Companies;

namespace FreyaManager.Features.Clinics;

public class CreateClinicViewModel : CoreViewModel
{
    private readonly ISessionService sessionService;
    private readonly INavigationService navigationService;
    private readonly IClinicService clinicService;
    private NewClinicModel newClinicModel;
    private Company company;

    public CreateClinicViewModel(
        ISessionService sessionService,
        INavigationService navigationService,
        IClinicService clinicService)
    {
        this.sessionService = sessionService;
        this.navigationService = navigationService;
        this.clinicService = clinicService;
        CreateClinicCommand = new AsyncCommand(CreateClinicCommandExecute);
        BackCommand = new AsyncCommand(BackCommandExecute);
        newClinicModel = new NewClinicModel();
        company = new();

        ShowBackButton = true;
        Title = "Nueva clinica";
    }

    public IAsyncCommand CreateClinicCommand { get; set; }
    public IAsyncCommand BackCommand { get; set; }


    public NewClinicModel NewClinicModel
    {
        get => newClinicModel;
        set
        {
            SetAndRaisePropertyChanged(ref newClinicModel, value);
        }
    }

    public override Task OnAppearingAsync(object? parameter = null)
    {
        company = sessionService.Get<Company>(SESSION.KEY_COMPANY_SELECTED);
        return base.OnAppearingAsync();
    }

    private async Task CreateClinicCommandExecute()
    {
        if (company.Id == Guid.Empty || string.IsNullOrWhiteSpace(NewClinicModel.Name))
        {
            
            return;
        }

        IsBusy = true;
        var result = await clinicService.CreateClinic(NewClinicModel.ToRequest(company.Id));

        IsBusy = false;

        if (result)
        {
            await navigationService.BackAsync();
        }
    }

    private Task BackCommandExecute()
    {
        return navigationService.BackAsync();
    }
}
