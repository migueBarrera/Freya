using Models.Core.Companies;
using Models.Core.Subscriptions;

namespace FreyaManager.Features.Companies;

public class DetailCompanyViewModel : CoreViewModel
{
    private readonly CompaniesService companiesService;
    private readonly INavigationService navigationService;
    private readonly ISessionService sessionService;
    private readonly IServiceProvider serviceProvider;
    private readonly IDialogService dialogService;
    private readonly ITranslatorService translatorService;
    private CompanyDetailResponse company;
    private IEnumerable<PaymentPlansForCompanyItemViewModel> subs = Enumerable.Empty<PaymentPlansForCompanyItemViewModel>();

    public DetailCompanyViewModel(
        INavigationService navigationService,
        ISessionService sessionService,
        CompaniesService companiesService,
        IServiceProvider serviceProvider,
        IDialogService dialogService,
        ITranslatorService translatorService,
        EmployeesForCompanyViewModel EmployeesForCompanyViewModel,
        ClinicsForCompanyViewModel ClinicsForCompanyViewModel)
    {
        Title = $"Ficha compañia";
        ShowBackButton = true;
        company = new CompanyDetailResponse();

        this.navigationService = navigationService;
        this.sessionService = sessionService;
        this.companiesService = companiesService;
        this.serviceProvider = serviceProvider;
        this.ClinicsForCompanyViewModel = ClinicsForCompanyViewModel;
        this.EmployeesForCompanyViewModel = EmployeesForCompanyViewModel;
        this.dialogService = dialogService;

        this.ClinicsForCompanyViewModel.Parent = this;
        this.EmployeesForCompanyViewModel.Parent = this;

        CreateEmployeeCommand = new AsyncCommand(CreateEmployeeCommandExecute);
        CreateClinicCommand = new AsyncCommand(CreateClinicCommandExecute);
        EditCompanyCommand = new AsyncCommand(EditCompanyCommandExecute);
        DeleteCompanyCommand = new AsyncCommand(DeleteCompanyCommandExecute);
        this.translatorService = translatorService;
    }

    public EmployeesForCompanyViewModel EmployeesForCompanyViewModel { get; set; }

    public ClinicsForCompanyViewModel ClinicsForCompanyViewModel { get; set; }

    public CompanyDetailResponse Company { get => company; set => SetAndRaisePropertyChanged(ref company, value); }

    public IEnumerable<PaymentPlansForCompanyItemViewModel> Subscriptions { get => subs; set => SetAndRaisePropertyChanged(ref subs, value); }

    public IAsyncCommand CreateEmployeeCommand { get; set; }

    public IAsyncCommand CreateClinicCommand { get; set; }

    public IAsyncCommand EditCompanyCommand { get; set; }

    public IAsyncCommand DeleteCompanyCommand { get; set; }

    public override async Task OnAppearingAsync(object? parameter = null)
    {
        await base.OnAppearingAsync();

        var _company = sessionService.Get<Company>(SESSION.KEY_COMPANY_SELECTED);
        
        await GetCompany(_company.Id);
    }

    private async Task GetCompany(Guid id)
    {
        IsBusy = true;
        Company = await companiesService.GetCompany(id);
        await ClinicsForCompanyViewModel.LoadClinics(company.Clinics, id);
        await EmployeesForCompanyViewModel.LoadEmployees(company.Employees, id);

        Subscriptions = await ConvertToSubscriptionPaymentViewModel(Company.PaymentsPlansForCompanyResponses);

        IsBusy = false;
    }

    private async Task<IEnumerable<PaymentPlansForCompanyItemViewModel>> ConvertToSubscriptionPaymentViewModel(IEnumerable<PaymentsPlansForCompanyResponse> items)
    {
        var viewmodels = new List<PaymentPlansForCompanyItemViewModel>();

        foreach (var item in items)
        {
            if (serviceProvider.GetService(typeof(PaymentPlansForCompanyItemViewModel)) is PaymentPlansForCompanyItemViewModel vm)
            {
                await vm.OnAppearingAsync(item);
                viewmodels.Add(vm);
            }
        }

        return new ObservableCollection<PaymentPlansForCompanyItemViewModel>(viewmodels);
    }

    private async Task CreateEmployeeCommandExecute()
    {
        sessionService.Save(SESSION.KEY_NEW_EMPLOYEE_FOR_CLINIC, false);
        sessionService.Save(SESSION.KEY_COMPANY_ID_SELECTED, company.Id);
        await navigationService.NavigateTo(typeof(CheckEmployeePage));
    }

    private async Task CreateClinicCommandExecute()
    {
        await navigationService.NavigateTo(typeof(CreateClinicPage));
    }

    private Task EditCompanyCommandExecute()
    {
        //No need pass company to session because is already set

        return navigationService.NavigateTo(typeof(EditCompanyPage));
    }

    private async Task DeleteCompanyCommandExecute()
    {
        await dialogService.ShowMessageYesNo(
                    "Eliminar compañia",
                    "¿Esta seguro que quiere eliminar esta compañia? Antes deberá eliminar todas las clinicas.",
                    new AsyncCommand(async () =>
                    {
                        IsBusy = true;
                        var deleted = await companiesService.DeleteCompany(company.Id);
                        IsBusy = false;
                        if (deleted)
                        {
                            await navigationService.BackAsync();
                        }
                    }));
    }

}
