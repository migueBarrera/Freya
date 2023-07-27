using Freya.Desktop.Core.Features.Clients;
using Models.Core.Common;

namespace Freya.Features.Clinics.Models;

public class ClinicDetailClientsViewModel : CoreViewModel
{
    private readonly IServiceProvider serviceProvider;
    private readonly ISessionService sessionService;
    private readonly INavigationService navigationService;
    private readonly ClientService clientService;
    private PagedModel<ClientItemViewModel> clients;
    public PaginationFilter paginationFilter = new PaginationFilter();

    public ClinicDetailClientsViewModel(
        IServiceProvider serviceProvider,
        ClientService clientService,
        ISessionService sessionService,
        INavigationService navigationService)
    {
        this.serviceProvider = serviceProvider;
        this.clientService = clientService;
        this.sessionService = sessionService;
        this.navigationService = navigationService;

        clients = PagedModel<ClientItemViewModel>.Empty();
        LessItemsCommand = new AsyncCommand(LessItemsCommandExecute);
        MoreItemsCommand = new AsyncCommand(MoreItemsCommandExecute);
    }

    public IAsyncCommand LessItemsCommand { get; set; }

    public IAsyncCommand MoreItemsCommand { get; set; }

    public PagedModel<ClientItemViewModel> Clients { get => clients; set => SetAndRaisePropertyChanged(ref clients, value); }

    internal async Task LoadClients(PagedModel<ClientResponse>? clients)
    {
        Clients = await ConvertToClientItemViewModel(clients ?? PagedModel<ClientResponse>.Empty());
        paginationFilter = new PaginationFilter(Clients.CurrentPage, Clients.PageSize);
    }

    private async Task MoreItemsCommandExecute()
    {
        paginationFilter.PageNumber++;
        await GetClientsAsync();
    }

    private async Task LessItemsCommandExecute()
    {
        paginationFilter.PageNumber--;
        await GetClientsAsync();
    }

    private async Task GetClientsAsync()
    {
        Parent!.IsBusy = true;
        var clients = await clientService.GetClients(paginationFilter);
        Clients = await ConvertToClientItemViewModel(clients);
        Parent!.IsBusy = false;
    }
    private async Task<PagedModel<ClientItemViewModel>> ConvertToClientItemViewModel(PagedModel<ClientResponse> items)
    {
        var viewmodels = PagedModel<ClientItemViewModel>.EmptyFrom<ClientResponse>(items);

        foreach (var item in items.Items)
        {
            if (serviceProvider.GetService(typeof(ClientItemViewModel)) is ClientItemViewModel vm)
            {
                vm.ViewProfileCommand = new AsyncCommand<ClientResponse>(ViewProfileClientCommandExecute);
                await vm.OnAppearingAsync(item);
                viewmodels.Items.Add(vm);
            }
        }

        return viewmodels;
    }

    private async Task ViewProfileClientCommandExecute(ClientResponse client)
    {
        sessionService.Save(SESSION.KEY_CLIENT_SELECTED, client);
        await navigationService.NavigateTo(typeof(ClientDetailPage));
    }

}
