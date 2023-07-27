using System.Windows.Input;

namespace FreyaManager.Features.Subscriptions;

public class SubscriptionsViewModel : CoreViewModel
{
    private readonly SubscriptionService subscriptionService;
    private IEnumerable<SubscriptionProductItemViewModel> subscriptionProducts;
    private readonly IServiceProvider serviceProvider;
    private readonly INavigationService navigationService;

    public SubscriptionsViewModel(
        SubscriptionService subscriptionService,
        IServiceProvider serviceProvider,
        INavigationService navigationService)
    {
        Title = "Subscripciones";
        subscriptionProducts = Enumerable.Empty<SubscriptionProductItemViewModel>();

        this.subscriptionService = subscriptionService;
        this.serviceProvider = serviceProvider;
        this.navigationService = navigationService;

        CreateNewSubscriptionCommand = new AsyncCommand(CreateNewSubscriptionCommandExecute);
    }

    public ICommand CreateNewSubscriptionCommand { get; set; }

    public override async Task OnAppearingAsync(object? parameter = null)
    {
        await base.OnAppearingAsync();
        await LoadSubscriptionProducts();
    }

    public IEnumerable<SubscriptionProductItemViewModel> SubscriptionProducts 
    { 
        get => subscriptionProducts; 
        set => SetAndRaisePropertyChanged(ref subscriptionProducts, value); 
    }

    private async Task CreateNewSubscriptionCommandExecute()
    {
        await navigationService.NavigateTo(typeof(NewSubscriptionPage));
    }

    private async Task LoadSubscriptionProducts()
    {
        IsBusy = true;
        var subs = await subscriptionService.GetSubscriptionProducts();
        var list = new List<SubscriptionProductItemViewModel>();
        foreach (var item in subs)
        {
            var vm = serviceProvider.GetService<SubscriptionProductItemViewModel>();
            if (vm != null)
            {
                await vm.OnAppearingAsync(item);
                list.Add(vm);
            }
        }
        SubscriptionProducts = list;
        IsBusy = false;
    }
}
