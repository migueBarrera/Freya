using Freya.Desktop.Core.Helpers;

namespace FreyaManager.Features.Companies;

public class NewCompanyViewModel : CoreViewModel
{
    private readonly CompaniesService _companiesService;
    private readonly INavigationService navigationService;
    private readonly IDialogService dialogService;
    private readonly Subscriptions.SubscriptionService subscriptionService;
    private string companyName;

    public NewCompanyViewModel(
        CompaniesService companiesService,
        INavigationService navigationService,
        Subscriptions.SubscriptionService subscriptionService,
        IDialogService dialogService)
    {
        ShowBackButton = true;
        Title = "Crear nueva compañia";
        companyName = string.Empty;

        _companiesService = companiesService;
        this.navigationService = navigationService;
        this.subscriptionService = subscriptionService;
        this.dialogService = dialogService;

        CreateCommand = new AsyncCommand(CreateCommandAsync);
        BackCommand = new AsyncCommand(BackCommandAsync);
    }

    public string CompanyName { get => companyName; set => SetAndRaisePropertyChanged(ref companyName, value); }

    public IAsyncCommand CreateCommand { get; set; }

    public IAsyncCommand BackCommand { get; set; }

    public ObservableCollection<CheckboxData> SubcriptionCheckboxes { get; set; } = new ObservableCollection<CheckboxData>();

    public override async Task OnAppearingAsync(object? parameter = null)
    {
        await base.OnAppearingAsync(parameter);

        await LoadSubscriptionChecks();
    }

    public async Task LoadSubscriptionChecks()
    {
        var items = await subscriptionService.GetSubscriptionProducts();

        foreach (var item in items)
        {
            SubcriptionCheckboxes.Add(new CheckboxData
            { 
                Id = item.Id, 
                Label = $"{item.Name} {SizeHelper.ConvertBytesToMB(item.Size)} MB" ,
            });
        }
    }

    private async Task CreateCommandAsync()
    {
        if (string.IsNullOrWhiteSpace(companyName))
        {
            await dialogService.ShowMessage(
                "Error",
                "Debes introducir un nombre");
            return;
        }

        var subscriptionSelected = SubcriptionCheckboxes.Where(x => x.Checked);
        if (!subscriptionSelected.Any())
        {
            await dialogService.ShowMessage(
                "Error", 
                "Debes seleccionar alguna subscripción");
            return;
        }

        IsBusy = true;

        var result = await _companiesService.CreateCompany(
            companyName, 
            subscriptionSelected.Select(x => x.Id));

        IsBusy = false;
        if (result)
        {
            await navigationService.BackAsync();
        }
    }

    private Task BackCommandAsync()
    {
        return navigationService.BackAsync();
    }
}

public class CheckboxData
{
    public Guid Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public bool Checked { get; set; }
}
