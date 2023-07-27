using System.Windows.Input;

namespace Freya.Features.Subscriptions;

public class SubscriptionClinicItemViewModel : CoreViewModel
{
    private readonly INavigationService navigationService;
    private readonly ISessionService sessionService;
    private bool canCancelateSubscription;
    private SubscriptionPaymentResponse? data;

    public SubscriptionClinicItemViewModel(
        INavigationService navigationService, 
        ISessionService sessionService)
    {
        SeeDetailSubscriptionPaymentsCommand = new AsyncCommand(GoToDetailCommandExcecute);
        this.navigationService = navigationService;
        this.sessionService = sessionService;
    }

    public SubscriptionPaymentResponse? Data { get => data; set => SetAndRaisePropertyChanged(ref data,value); }

    public ICommand SeeDetailSubscriptionPaymentsCommand { get; set; }

    public bool CanCancelateSubscription { get => canCancelateSubscription; set => SetAndRaisePropertyChanged(ref canCancelateSubscription, value); }

    public override Task OnAppearingAsync(object? parameter = null)
    {
        Data = (parameter as SubscriptionPaymentResponse) ?? new SubscriptionPaymentResponse();
        return base.OnAppearingAsync(parameter);
    }

    private Task GoToDetailCommandExcecute()
    {
        sessionService.Save(SESSION.KEY_PAYMENT_ID_SELECTED, data?.Id);
        return navigationService.NavigateTo(typeof(SubscriptionPaymentDetailPage));
    }
}
