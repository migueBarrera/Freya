using FreyaManager.Features.Clients;
using Models.Core.Common;

namespace FreyaManager.Features.Clinics.Models
{
    public class ClinicDetailClientsViewModel : CoreViewModel
    {
        private readonly IServiceProvider serviceProvider;
        private readonly INavigationService navigationService;
        private readonly ISessionService sessionService;
        private readonly ClientService clientService;
        private PagedModel<ClientItemViewModel> clients;
        private Guid clinicId;
        public PaginationFilter paginationFilter = new PaginationFilter();

        public ClinicDetailClientsViewModel(
            ClientService clientService,
            IServiceProvider serviceProvider,
            INavigationService navigationService,
            ISessionService sessionService)
        {
            this.clientService = clientService;
            this.serviceProvider = serviceProvider;
            this.navigationService = navigationService;
            this.sessionService = sessionService;
            clients = PagedModel<ClientItemViewModel>.Empty();
            LessItemsCommand = new AsyncCommand(LessItemsCommandExecute);
            MoreItemsCommand = new AsyncCommand(MoreItemsCommandExecute);
        }

        public IAsyncCommand LessItemsCommand { get; set; }

        public IAsyncCommand MoreItemsCommand { get; set; }

        public PagedModel<ClientItemViewModel> Clients { get => clients; set => SetAndRaisePropertyChanged(ref clients, value); }

        internal async Task LoadClients(PagedModel<ClientResponse>? employees, Guid clinicId)
        {
            this.clinicId = clinicId;
            Clients = await ConvertToEmployeeItemViewModel(employees ?? PagedModel<ClientResponse>.Empty());
            paginationFilter = new PaginationFilter(Clients.CurrentPage, Clients.PageSize);
        }

        private async Task<PagedModel<ClientItemViewModel>> ConvertToEmployeeItemViewModel(PagedModel<ClientResponse> items)
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
            var clients = await clientService.GetClients(clinicId, paginationFilter);
            Clients = await ConvertToEmployeeItemViewModel(clients);
            Parent!.IsBusy = false;
        }

        private async Task ViewProfileClientCommandExecute(ClientResponse client)
        {
            sessionService.Save(SESSION.KEY_CLIENT_SELECTED, client);
            await navigationService.NavigateTo(typeof(ClientDetailPage));
        }
    }

}
