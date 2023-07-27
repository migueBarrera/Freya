using Models.Core.Subscriptions;

namespace FreyaManager.Features.Subscriptions.Charges;

public class ChargesViewModel : CoreViewModel
{
    private readonly SubscriptionChargesService subscriptionChargesService;
    private readonly ISessionService sessionService;
    private readonly IServiceProvider serviceProvider;
    private List<SubscriptionChargeResponse> subscriptionCharges = new List<SubscriptionChargeResponse>();
    private SubscriptionPaymentViewModel? currenSupscription;


    public ChargesViewModel(SubscriptionChargesService subscriptionChargesService, ISessionService sessionService, IServiceProvider serviceProvider)
    {
        Title = "Cargos de la subscripción";
        ShowBackButton = true;
        this.subscriptionChargesService = subscriptionChargesService;
        this.sessionService = sessionService;
        this.serviceProvider = serviceProvider;
    }

    public List<SubscriptionChargeResponse> SubscriptionCharges
    {
        get => subscriptionCharges;
        set=> SetAndRaisePropertyChanged(ref subscriptionCharges, value);
    }
    public SubscriptionPaymentViewModel? CurrenSupscription { get => currenSupscription; set => SetAndRaisePropertyChanged(ref currenSupscription, value); }

    public override async Task OnAppearingAsync(object? parameter = null)
    {
        await base.OnAppearingAsync(parameter);

        var paymentId = sessionService.Get<Guid>(SESSION.KEY_PAYMENT_ID_SELECTED);

        await LoadCarges(paymentId);
    }

    private async Task LoadCarges(Guid paymentId)
    {
        IsBusy = true;
        var item = await subscriptionChargesService.CreateSubscriptionProduct(paymentId);
        IsBusy = false;

        if (item != null)
        {
            if (serviceProvider.GetService(typeof(SubscriptionPaymentViewModel)) is SubscriptionPaymentViewModel vm)
            {
                await vm.OnAppearingAsync(item);
                CurrenSupscription = vm;
            }
             
            SubscriptionCharges = item.SubscriptionCharges.ToList();
        }
    }
}
