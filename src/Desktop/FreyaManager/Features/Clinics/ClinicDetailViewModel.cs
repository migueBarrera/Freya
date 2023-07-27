using Models.Core.Clinics;

namespace FreyaManager.Features.Clinics;

public class ClinicDetailViewModel : CoreViewModel
{
    private readonly IClinicService clinicService;
    private readonly INavigationService navigationService;
    private readonly ISessionService sessionService;
    private readonly ITranslatorService translatorService;
    private readonly IDialogService dialogService;
    private readonly AppCenterAnalitics appCenterAnalitics;
    private readonly IServiceProvider serviceProvider;
    private ClinicDetailResponse clinic = new ClinicDetailResponse();
    private List<SubscriptionPaymentViewModel> subscriptionPayments = new List<SubscriptionPaymentViewModel>();

    public ClinicDetailViewModel(
        IClinicService clinicService,
        INavigationService navigationService,
        ISessionService sessionService,
        ClinicDetailEmployeesViewModel clinicDetailEmployeesViewModel,
        ClinicDetailClientsViewModel clinicDetailClientsViewModel,
        IServiceProvider serviceProvider,
        ITranslatorService translatorService,
        IDialogService dialogService,
        AppCenterAnalitics appCenterAnalitics)
    {
        Title = $"Ficha clínica";
        ShowBackButton = true;

        this.clinicService = clinicService;
        this.navigationService = navigationService;
        this.sessionService = sessionService;
        this.serviceProvider = serviceProvider;
        this.translatorService = translatorService;

        CreateEmployeeCommand = new AsyncCommand(CreateEmployeeCommandExecute);
        DeleteClinicCommand = new AsyncCommand(DeleteClinicCommandExecute);
        EditClinicCommand = new AsyncCommand(EditClinicCommandExecute);

        ClinicDetailEmployeesViewModel = clinicDetailEmployeesViewModel;
        ClinicDetailClientsViewModel = clinicDetailClientsViewModel;

        ClinicDetailEmployeesViewModel.Parent = this;
        ClinicDetailClientsViewModel.Parent = this;
        this.dialogService = dialogService;
        this.appCenterAnalitics = appCenterAnalitics;
    }

    public ClinicDetailEmployeesViewModel ClinicDetailEmployeesViewModel { get; set; }

    public ClinicDetailClientsViewModel ClinicDetailClientsViewModel { get; set; }

    public IAsyncCommand DeleteClinicCommand { get; set; }

    public IAsyncCommand EditClinicCommand { get; set; }

    public IAsyncCommand CreateEmployeeCommand { get; set; }

    public ClinicDetailResponse Clinic { get => clinic; set => SetAndRaisePropertyChanged(ref clinic, value); }

    public List<SubscriptionPaymentViewModel> SubscriptionPayments { get => subscriptionPayments; set => SetAndRaisePropertyChanged(ref subscriptionPayments, value); }

    public override async Task OnAppearingAsync(object? parameter = null)
    {
        await base.OnAppearingAsync();
        var clinic = sessionService.Get<ClinicResponse>(SESSION.KEY_CLINIC_SELECTED);
        await GetClinic(clinic.Id);
    }

    private async Task GetClinic(Guid id)
    {
        IsBusy = true;
        sessionService.Save(SESSION.KEY_CLINIC_ID_SELECTED, id);
        Clinic = await clinicService.GetClinic(id);
        await ClinicDetailEmployeesViewModel.LoadEmployees(Clinic.Employees, Clinic.Id);
        await ClinicDetailClientsViewModel.LoadClients(Clinic.Clients, Clinic.Id);
        await LoadSubscriptionPayments();

        IsBusy = false;
    }

    private async Task LoadSubscriptionPayments()
    {
        List<SubscriptionPaymentViewModel> list = new();

        foreach (var item in Clinic.SubscriptionPaymentResponses) 
        {
            if (serviceProvider.GetService(typeof(SubscriptionPaymentViewModel)) is SubscriptionPaymentViewModel vm)
            {
                await vm.OnAppearingAsync(item);
                list.Add(vm);
            }
        }

        SubscriptionPayments = list;
    }

    private async Task CreateEmployeeCommandExecute()
    {
        sessionService.Save(SESSION.KEY_NEW_EMPLOYEE_FOR_CLINIC, true);
        sessionService.Save(SESSION.KEY_NEW_EMPLOYEE_FOR_CLINIC_ID, clinic.Id);
        await navigationService.NavigateTo(typeof(CheckEmployeePage));
    }

    private async Task EditClinicCommandExecute()
    {
        sessionService.Save(SESSION.KEY_CLINIC_DETAIL_SELECTED, Clinic);
        await navigationService.NavigateTo(typeof(EditClinicPage));
    }

    private async Task DeleteClinicCommandExecute()
    {
        appCenterAnalitics.Clicked("Delete Clinic");
        await dialogService.ShowMessageYesNo(
                    translatorService.Translate("dialog_clinic_detail_remove_clinic_title"),
                    translatorService.Translate("dialog_clinic_detail_remove_clinic_body"),
                    new AsyncCommand(async () =>
                    {
                        IsBusy = true;
                        var deleted = await clinicService.DeleteClinic(Clinic!.Id);
                        IsBusy = false;
                        if (deleted)
                        {
                            await navigationService.BackAsync();
                        }
                    }));
    }
}
