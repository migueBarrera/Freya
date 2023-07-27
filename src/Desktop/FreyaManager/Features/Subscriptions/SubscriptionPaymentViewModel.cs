using Models.Core.Subscriptions;

namespace FreyaManager.Features.Subscriptions;

public class SubscriptionPaymentViewModel : CoreViewModel
{
    private readonly ISessionService sessionService;
    private readonly INavigationService navigationService;
    private readonly IConfiguration configuration;
    private SubscriptionPaymentResponse? subscriptionPaymentResponse;

    public SubscriptionPaymentViewModel(ISessionService sessionService, INavigationService navigationService, IConfiguration configuration)
    {
        SeeSubscriptionOnStripeCommand = new AsyncCommand(SeeSubscriptionOnStripeCommandExecute);
        SeeSubscriptionChargesCommand = new AsyncCommand(SeeSubscriptionChargesCommandExecute);
        this.sessionService = sessionService;
        this.navigationService = navigationService;
        this.configuration = configuration;
    }

    public IAsyncCommand SeeSubscriptionOnStripeCommand { get; set; }

    public IAsyncCommand SeeSubscriptionChargesCommand { get; set; }

    public override Task OnAppearingAsync(object? parameter = null)
    {
        SubscriptionPaymentResponse = (parameter as SubscriptionPaymentResponse) ?? new SubscriptionPaymentResponse();
        return Task.CompletedTask;
    }

    public SubscriptionPaymentResponse? SubscriptionPaymentResponse 
    { 
        get => subscriptionPaymentResponse; 
        set => SetAndRaisePropertyChanged(ref subscriptionPaymentResponse, value); 
    }

    private Task SeeSubscriptionOnStripeCommandExecute()
    {
        bool isProductionEvironment = configuration.GetValue<bool>("IsProductionEvironment");
        string target = "https://dashboard.stripe.com/test/customers/";
        if (isProductionEvironment)
        {
            target = "https://dashboard.stripe.com/customers/";
        }

        var subs = SubscriptionPaymentResponse?.StripeCustomerId;

        var sInfo = new System.Diagnostics.ProcessStartInfo(target + subs)
        {
            UseShellExecute = true,
        };
        System.Diagnostics.Process.Start(sInfo);
        return Task.CompletedTask;
    }

    private Task SeeSubscriptionChargesCommandExecute()
    {
        sessionService.Save(SESSION.KEY_PAYMENT_ID_SELECTED, SubscriptionPaymentResponse?.Id);
        return navigationService.NavigateTo(typeof(ChargesPage));
    }
}
