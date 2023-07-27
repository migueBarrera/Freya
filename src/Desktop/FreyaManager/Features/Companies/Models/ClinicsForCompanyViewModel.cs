using Models.Core.Clinics;
using Models.Core.Common;

namespace FreyaManager.Features.Companies.Models;

public class ClinicsForCompanyViewModel : CoreViewModel
{
    private readonly IClinicService clinicService;
    private readonly INavigationService navigationService;
    private readonly ISessionService sessionService;
    private PagedModel<ClinicModel> clinics;
    public PaginationFilter paginationFilter = new PaginationFilter();
    public Guid companyId = Guid.Empty;

    public ClinicsForCompanyViewModel(
        IClinicService clinicService,
        INavigationService navigationService,
        ISessionService sessionService)
    {
        clinics = PagedModel<ClinicModel>.Empty();
        LessItemsCommand = new AsyncCommand(LessItemsCommandExecute);
        MoreItemsCommand = new AsyncCommand(MoreItemsCommandExecute);
        this.clinicService = clinicService;
        this.navigationService = navigationService;
        this.sessionService = sessionService;
    }

    public IAsyncCommand LessItemsCommand { get; set; }

    public IAsyncCommand MoreItemsCommand { get; set; }

    public PagedModel<ClinicModel> Clinics { get => clinics; set => SetAndRaisePropertyChanged(ref clinics, value); }

    internal Task LoadClinics(PagedModel<ClinicResponse>? employees, Guid companyId)
    {
        this.companyId = companyId;
        Clinics = ConvertToEmployeeItemViewModel(employees ?? PagedModel<ClinicResponse>.Empty());
        paginationFilter = new PaginationFilter(Clinics.CurrentPage, Clinics.PageSize);

        return Task.CompletedTask;
    }

    private PagedModel<ClinicModel> ConvertToEmployeeItemViewModel(PagedModel<ClinicResponse> items)
    {
        var viewmodels = PagedModel<ClinicModel>.EmptyFrom<ClinicResponse>(items);

        foreach (var item in items.Items)
        {
            viewmodels.Items.Add(new ClinicModel(item, navigationService, sessionService));
        }

        return viewmodels;
    }

    private async Task MoreItemsCommandExecute()
    {
        paginationFilter.PageNumber++;
        await GetEmployeesAsync();
    }

    private async Task LessItemsCommandExecute()
    {
        paginationFilter.PageNumber--;
        await GetEmployeesAsync();
    }

    private async Task GetEmployeesAsync()
    {
        Parent!.IsBusy = true;
        var employees = await clinicService.GetClinicsByCompanyId(companyId, paginationFilter);
        Clinics = ConvertToEmployeeItemViewModel(employees);
        Parent!.IsBusy = false;
    }
}
