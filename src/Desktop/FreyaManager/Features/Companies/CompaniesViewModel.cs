using Models.Core.Common;

namespace FreyaManager.Features.Companies;

public class CompaniesViewModel : CoreViewModel
{
    private readonly CompaniesService _companiesService;
    private readonly INavigationService navigationService;
    private PagedModel<CompanyModel> companies;

    public CompaniesViewModel(CompaniesService companiesService, INavigationService navigationService)
    {
        _companiesService = companiesService;
        companies = PagedModel<CompanyModel>.Empty();

        NewCompanyCommand = new AsyncCommand(NewCompanyCommandExecute);
        LessItemsCommand = new AsyncCommand(LessItemsCommandExecute);
        MoreItemsCommand = new AsyncCommand(MoreItemsCommandExecute);
        this.navigationService = navigationService;

        Title = "Compañias";

        PaginationFilter = new PaginationFilter();
    }

    private async Task MoreItemsCommandExecute()
    {
        PaginationFilter.PageNumber++;
        await LoadCompanies();
    }

    private async Task LessItemsCommandExecute()
    {
        PaginationFilter.PageNumber--;
        await LoadCompanies();
    }

    public IAsyncCommand NewCompanyCommand { get; set; }
    public IAsyncCommand LessItemsCommand { get; set; }
    public IAsyncCommand MoreItemsCommand { get; set; }

    public PaginationFilter PaginationFilter { get; set; }

    public PagedModel<CompanyModel> Companies { get => companies; private set => SetAndRaisePropertyChanged(ref companies, value); }

    public override async Task OnAppearingAsync(object? parameter = null)
    {
        await base.OnAppearingAsync();
        await LoadCompanies();
    }

    private async Task LoadCompanies()
    {
        IsBusy = true;
        var list = await _companiesService.GetCompanies(PaginationFilter);
        IsBusy = false;
        Companies = list;
    }

    private Task NewCompanyCommandExecute()
    {
        return navigationService.NavigateTo(typeof(NewCompanyPage));
    }
}
