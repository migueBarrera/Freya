using Freya.Desktop.Core.Features.Clients;
using Freya.Desktop.Localization.Services;
using Models.Core.Common;

namespace Freya.Features.Clients;

public class ClientsViewModel : CoreViewModel
{
    private PagedModel<ClientItemViewModel> filteredClients;
    private string searchText;
    private readonly ClientService clientService;
    private readonly INavigationService navigationService;
    private readonly ICurrentClinicService currentClinicService;
    private readonly ISessionService sessionService;
    private readonly IServiceProvider serviceProvider;
    private readonly ITranslatorService translatorService;
    private readonly IDialogService dialogService;
    private readonly AppCenterAnalitics appCenterAnalitics;
    public PaginationFilter paginationFilter = new PaginationFilter();

    public ClientsViewModel(
        ClientService clientService,
        INavigationService navigationService,
        ICurrentClinicService currentClinicService,
        ISessionService sessionService,
        IServiceProvider serviceProvider,
        ITranslatorService translatorService,
        IDialogService dialogService,
        AppCenterAnalitics appCenterAnalitics)
    {
        filteredClients = PagedModel<ClientItemViewModel>.Empty();
        this.clientService = clientService;
        searchText = string.Empty;
        AddClientCommand = new AsyncCommand(AddClientCommandExecute);
        LessItemsCommand = new AsyncCommand(LessItemsCommandExecute);
        MoreItemsCommand = new AsyncCommand(MoreItemsCommandExecute);
        SearchCommand = new AsyncCommand<string>(SearchCommandExecute);
        this.navigationService = navigationService;
        Title = translatorService.Translate("clients");
        this.currentClinicService = currentClinicService;
        this.ShowClinicSelector = true;
        this.sessionService = sessionService;
        this.serviceProvider = serviceProvider;
        this.translatorService = translatorService;
        this.dialogService = dialogService;
        this.appCenterAnalitics = appCenterAnalitics;

        appCenterAnalitics.PageView(nameof(ClientsViewModel));
    }

    public IAsyncCommand AddClientCommand { get; set; }

    public IAsyncCommand<string> SearchCommand { get; set; }

    public IAsyncCommand LessItemsCommand { get; set; }

    public IAsyncCommand MoreItemsCommand { get; set; }

    public PagedModel<ClientItemViewModel> FilteredClients 
    { 
        get => filteredClients; 
        set => SetAndRaisePropertyChanged(ref filteredClients, value); 
    }

    public override async Task OnAppearingAsync(object? parameter = null)
    {
        await base.OnAppearingAsync();
        await GetClientsAsync();
        currentClinicService.OnCurrentClinicSelectedChanged += OnCurrentClinicSelectedChanged;
    }

    public override Task OnDisappearingAsync()
    {
        currentClinicService.OnCurrentClinicSelectedChanged -= OnCurrentClinicSelectedChanged;
        return base.OnDisappearingAsync();
    }

    private async void OnCurrentClinicSelectedChanged(object? sender, ClinicResponse? e)
    {
        await GetClientsAsync();
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

    private async Task GetClientsAsync()
    {
        sessionService.Save(SESSION.KEY_CLINIC_ID_SELECTED, currentClinicService.CurrentClinic?.Id);
        IsBusy = true;
        var clients = await clientService.GetClients(paginationFilter);
        IsBusy = false;

        var list = PagedModel<ClientItemViewModel>.EmptyFrom(clients);
        foreach (var item in clients.Items)
        {
            if (serviceProvider.GetService(typeof(ClientItemViewModel)) is ClientItemViewModel vm)
            {
                vm.ViewProfileCommand = new AsyncCommand<ClientResponse>(ViewProfileClientCommandExecute);
                await vm.OnAppearingAsync(item);
                list.Items.Add(vm);
            }
        }

        FilteredClients = list;
    }

    private async Task AddClientCommandExecute()
    {
        if (currentClinicService.CurrentClinic == null)
        {
            await dialogService.ShowMessage(
                translatorService.Translate("dialog_client_you_need_create_a_clinic_before_a_client_title"), 
                translatorService.Translate("dialog_client_you_need_create_a_clinic_before_a_client_body"));
            return;
        }

        appCenterAnalitics.Clicked("Add client from clinic list");
        sessionService.Save(SESSION.KEY_CLINIC_ID_SELECTED, currentClinicService.CurrentClinic?.Id);
        await navigationService.NavigateTo(typeof(CheckClientPage));
    }

    private async Task MoreItemsCommandExecute()
    {
        appCenterAnalitics.Clicked("Clients next page");
        paginationFilter.PageNumber++;
        await GetClientsAsync();
    }

    private async Task LessItemsCommandExecute()
    {
        appCenterAnalitics.Clicked("Clients previous page");
        paginationFilter.PageNumber--;
        await GetClientsAsync();
    }
    
    private async Task SearchCommandExecute(string text)
    {
        appCenterAnalitics.Clicked("Search Clients");
        paginationFilter = new PaginationFilter();

        if (!string.IsNullOrWhiteSpace(text))
        {
            paginationFilter.SearchArgument = text;
        }

        await GetClientsAsync();
    }

    private async Task ViewProfileClientCommandExecute(ClientResponse client)
    {
        sessionService.Save(SESSION.KEY_CLIENT_SELECTED, client);
        await navigationService.NavigateTo(typeof(ClientDetailPage));
    }
}
