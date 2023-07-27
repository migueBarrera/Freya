using Models.Core.Common;
using System.ComponentModel;
using System.Windows.Data;

namespace Freya.Features.Clinics;

public class ClinicsViewModel : CoreViewModel
{
    private readonly IClinicsService clinicsService;
    private readonly INavigationService navigationService;
    private readonly IServiceProvider serviceProvider;
    private readonly AppCenterAnalitics appCenterAnalitics;
    private PagedModel<ClinicItemViewModel> clinics;
    private string searchText;
    public PaginationFilter paginationFilter = new PaginationFilter();

    public ClinicsViewModel(
        IClinicsService clinicsService,
        INavigationService navigationService,
        IServiceProvider serviceProvider,
        ITranslatorService translatorService,
        AppCenterAnalitics appCenterAnalitics)
    {
        this.clinicsService = clinicsService;
        Title = translatorService.Translate("page_title_clinics");
        clinics = PagedModel<ClinicItemViewModel>.Empty();
        searchText = string.Empty;
        CreateClinicCommand = new AsyncCommand(CreateClinicCommandExecute);
        LessItemsCommand = new AsyncCommand(LessItemsCommandExecute);
        MoreItemsCommand = new AsyncCommand(MoreItemsCommandExecute);
        SearchCommand = new AsyncCommand<string>(SearchCommandExecute);
        this.navigationService = navigationService;
        this.serviceProvider = serviceProvider;
        this.appCenterAnalitics = appCenterAnalitics;

        appCenterAnalitics.PageView(nameof(ClinicsViewModel));
    }

    public IAsyncCommand<string> SearchCommand { get; set; }

    public IAsyncCommand LessItemsCommand { get; set; }

    public IAsyncCommand MoreItemsCommand { get; set; }

    public IAsyncCommand CreateClinicCommand { get; set; }

    public PagedModel<ClinicItemViewModel> Clinics
    {
        get => clinics;
        set => SetAndRaisePropertyChanged(ref clinics, value);
    }

    public string SearchText
    {
        get
        {
            return searchText;
        }

        set
        {
            SetAndRaisePropertyChangedIfDifferentValues(ref searchText, value);
        }
    }

    public override async Task OnAppearingAsync(object? parameter = null)
    {
        await base.OnAppearingAsync();
        await GetClinicsAsync();
    }

    private async Task GetClinicsAsync()
    {
        IsBusy = true;
        var clinics =  await clinicsService.GetClinicsAsync(paginationFilter);
        IsBusy = false;

        var list = PagedModel<ClinicItemViewModel>.EmptyFrom(clinics);
        foreach (var item in clinics.Items)
        {
            if (serviceProvider.GetService(typeof(ClinicItemViewModel)) is ClinicItemViewModel vm)
            {
                await vm.OnAppearingAsync(item);
                list.Items.Add(vm);
            }
        }

        Clinics = list;
    }

    private async Task CreateClinicCommandExecute()
    {
        await navigationService.NavigateTo(typeof(CreateNewClinicPage));
    }

    private async Task MoreItemsCommandExecute()
    {
        paginationFilter.PageNumber++;
        await GetClinicsAsync();
    }

    private async Task LessItemsCommandExecute()
    {
        paginationFilter.PageNumber--;
        await GetClinicsAsync();
    }

    private async Task SearchCommandExecute(string text)
    {
        paginationFilter = new PaginationFilter();

        if (!string.IsNullOrWhiteSpace(text))
        {
            paginationFilter.SearchArgument = text;
        }

        await GetClinicsAsync();
    }
}
