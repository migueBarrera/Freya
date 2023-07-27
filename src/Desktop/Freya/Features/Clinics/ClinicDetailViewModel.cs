using AppCenterServices;
using Freya.Desktop.Core.Features.Clients;
using Models.Core.Common;

namespace Freya.Features.Clinics;

public class ClinicDetailViewModel : CoreViewModel
{
    private ISessionService sessionService;
    private ICurrentClinicService currentClinicService;
    private IClinicDetailService clientDetailService;
    private IServiceProvider serviceProvider;
    private IDialogService dialogService;
    private INavigationService navigationService;
    private IEmployeeRolService employeeRolService;
    private ICurrentEmployeeService currentEmployeeService;
    private AppCenterAnalitics appCenterAnalitics;
    private ClinicDetailResponse? clinic;
    private PagedModel<ClientItemViewModel> clients;
    private SubscriptionClinicItemViewModel? subscriptionClinics;
    private IEnumerable<SubscriptionProductItemViewModel> subscripcions = Enumerable.Empty<SubscriptionProductItemViewModel>();
    private ITranslatorService translatorService;
    private bool isCompanyAdmin;
    private bool isClinicOfficer;

    public ClinicDetailViewModel(
        ISessionService sessionService,
        IClinicDetailService clientDetailService,
        ICurrentClinicService currentClinicService,
        IServiceProvider serviceProvider,
        IDialogService dialogService,
        INavigationService navigationService,
        IEmployeeRolService employeeRolService,
        ICurrentEmployeeService currentEmployeeService,
        ITranslatorService translatorService,
        ClinicDetailEmployeesViewModel clinicDetailEmployeesViewModel,
        ClinicDetailClientsViewModel clinicDetailClientsViewModel,
        AppCenterAnalitics appCenterAnalitics)
    {
        Title = translatorService.Translate("page_title_clinic_detail");
        ShowBackButton = false;
        ShowClinicSelector = true;
        this.sessionService = sessionService;
        AddEmployeeCommand = new AsyncCommand(AddEmployeeCommandExecute);
        AddClientCommand = new AsyncCommand(AddClientCommandExecute);
        DeleteClinicCommand = new AsyncCommand(DeleteClinicCommandExecute);
        EditClinicCommand = new AsyncCommand(EditClinicCommandExecute);
        this.clientDetailService = clientDetailService;
        this.currentClinicService = currentClinicService;


        clients = PagedModel<ClientItemViewModel>.Empty();
        this.serviceProvider = serviceProvider;
        this.dialogService = dialogService;
        this.navigationService = navigationService;
        this.employeeRolService = employeeRolService;
        this.currentEmployeeService = currentEmployeeService;
        this.translatorService = translatorService;
        this.appCenterAnalitics = appCenterAnalitics;
        ClinicDetailEmployeesViewModel = clinicDetailEmployeesViewModel;
        ClinicDetailClientsViewModel = clinicDetailClientsViewModel;

        ClinicDetailEmployeesViewModel.Parent = this;
        ClinicDetailClientsViewModel.Parent = this;

        appCenterAnalitics.PageView(nameof(ClinicDetailViewModel));
    }

    public ClinicDetailEmployeesViewModel ClinicDetailEmployeesViewModel { get; set; }

    public ClinicDetailClientsViewModel ClinicDetailClientsViewModel { get; set; }

    public IAsyncCommand AddEmployeeCommand { get; set; }

    public IAsyncCommand AddClientCommand { get; set; }

    public IAsyncCommand DeleteClinicCommand { get; set; }

    public IAsyncCommand EditClinicCommand { get; set; }

    public ClinicDetailResponse? Clinic { get => clinic; set => SetAndRaisePropertyChanged(ref clinic, value); }

    public PagedModel<ClientItemViewModel> Clients { get => clients; set => SetAndRaisePropertyChanged(ref clients, value); }

    public SubscriptionClinicItemViewModel? SubscriptionsClinic { get => subscriptionClinics; set => SetAndRaisePropertyChanged(ref subscriptionClinics, value); }

    public IEnumerable<SubscriptionProductItemViewModel> Subscripcions { get => subscripcions; set => SetAndRaisePropertyChanged(ref subscripcions, value); }

    public bool IsCompanyAdmin
    {
        get
        {
            return isCompanyAdmin;
        }
        set
        {
            SetAndRaisePropertyChanged(ref isCompanyAdmin, value);
            ShowBackButton = value;
        }
    }

    public bool IsClinicOfficer
    {
        get
        {
            return isClinicOfficer;
        }
        set
        {
            SetAndRaisePropertyChanged(ref isClinicOfficer, value);
        }
    }

    public override async Task OnAppearingAsync(object? parameter = null)
    {
        await base.OnAppearingAsync();
        IsCompanyAdmin = employeeRolService.IsCompanyAdmin(currentEmployeeService.Employee?.Rol ?? EmployeeRol.CLINIC_OFFICER);
        IsClinicOfficer = !employeeRolService.IsClinicManagerOrHigher(currentEmployeeService.Employee?.Rol ?? EmployeeRol.CLINIC_OFFICER);

        var clinicSessionId = sessionService.Get<Guid>(SESSION.KEY_CLINIC_ID_SELECTED);
        var name = sessionService.Get<string>(SESSION.KEY_CLINIC_NAME_SELECTED);

        (Parent as MainViewModel)?.HeaderControlViewModel.SetClinic(clinicSessionId, name);
        currentClinicService.OnCurrentClinicSelectedChanged += OnCurrentClinicSelectedChanged;

        await LoadClinicDataAndSubscriptionProducts(clinicSessionId);
    }

    public override Task OnDisappearingAsync()
    {
        currentClinicService.OnCurrentClinicSelectedChanged -= OnCurrentClinicSelectedChanged;
        return base.OnDisappearingAsync();
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
                        var deleted = await clientDetailService.DeleteClinic(Clinic!.Id);
                        IsBusy = false;
                        if (deleted)
                        {
                            await navigationService.BackAsync();
                            (Parent as MainViewModel)?.HeaderControlViewModel.RemoveClinic(Clinic.Id);
                        }
                    }));
    }

    private async Task AddEmployeeCommandExecute()
    {
        appCenterAnalitics.Clicked("Add employee from clinic detail");
        sessionService.Save(SESSION.KEY_CLINIC_ID_SELECTED, Clinic!.Id);
        await navigationService.NavigateTo(typeof(CheckEmployeePage));
    }

    private async Task AddClientCommandExecute()
    {
        appCenterAnalitics.Clicked("Add client from clinic detail");
        sessionService.Save(SESSION.KEY_CLINIC_ID_SELECTED, Clinic!.Id);
        await navigationService.NavigateTo(typeof(CheckClientPage));
    }

    private async Task LoadClinicDataAndSubscriptionProducts(Guid clinicSessionId)
    {
        IsBusy = true;
        await LoadMultimediaData(clinicSessionId);
        IsBusy = false;
    }

    private async void OnCurrentClinicSelectedChanged(object? sender, ClinicResponse? e)
    {
        if (e == null)
        {
            return;
        }

        sessionService.Save(SESSION.KEY_CLINIC_ID_SELECTED, e!.Id);
        await LoadClinicDataAndSubscriptionProducts(e!.Id);
    }

    private async Task<IEnumerable<SubscriptionProductItemViewModel>> ConvertToSubscriptionProductItemViewModel(IEnumerable<PaymentsPlansForCompanyResponse> items)
    {
        var viewmodels = new List<SubscriptionProductItemViewModel>();

        var clinicHasActiveSubscription = false;
        if (Clinic!.CurrentSubscription != null && Clinic.CurrentSubscription.State == SubscriptionStates.ACTIVE)
        {
            clinicHasActiveSubscription = true;
        }

        foreach (var item in items)
        {
            if (serviceProvider.GetService(typeof(SubscriptionProductItemViewModel)) is SubscriptionProductItemViewModel vm)
            {
                vm.ClinicHasASubcriptionActive = clinicHasActiveSubscription;
                vm.IsSubscriptionActive = clinicHasActiveSubscription
                        && item.ProductId.Equals(Clinic?.CurrentSubscription?.ProductId);
                await vm.OnAppearingAsync(item);
                viewmodels.Add(vm);
            }
        }

        return new ObservableCollection<SubscriptionProductItemViewModel>(viewmodels);
    }

    private async Task LoadMultimediaData(Guid clinicId)
    {
        var clinicResponse = await clientDetailService.GetClinic(clinicId);

        Clinic = clinicResponse;
        Subscripcions = await ConvertToSubscriptionProductItemViewModel(clinicResponse.PaymentsPlansForCompanies);
        await ClinicDetailEmployeesViewModel.LoadEmployees(clinicResponse.Employees);
        await ClinicDetailClientsViewModel.LoadClients(clinicResponse.Clients);

        if (Clinic.CurrentSubscription != null)
        {
            if (serviceProvider.GetService(typeof(SubscriptionClinicItemViewModel)) is SubscriptionClinicItemViewModel vm)
            {
                await vm.OnAppearingAsync(Clinic.CurrentSubscription);
                SubscriptionsClinic = vm;
            }
        }
        else
        {
            SubscriptionsClinic = null;
        }

        //update current subscription for header selector,
        //this is need for has updated data subscription of clinics
        if (currentClinicService.CurrentClinic != null)
        {
            currentClinicService.CurrentClinic.CurrentSubscription = Clinic.CurrentSubscription;
        }
    }

    private async Task EditClinicCommandExecute()
    {
        sessionService.Save(SESSION.KEY_CLINIC_SELECTED, Clinic);
        await navigationService.NavigateTo(typeof(EditClinicPage));
    }
}
