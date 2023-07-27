using Freya.Desktop.Core.Features.SubscriptionCharges;

namespace Freya.Features.Subscriptions;

public class SubscriptionPaymentDetailViewModel : CoreViewModel
{
    private readonly SubscriptionChargesService subscriptionChargesService;
    private readonly ISessionService sessionService;
    private readonly IDialogService dialogService;
    private readonly INavigationService navigationService;
    private readonly ITranslatorService translatorService;
    private readonly IClinicDetailService clinicDetailService;
    private readonly AppCenterAnalitics analitics;
    private List<SubscriptionChargeResponse> subscriptionCharges = new List<SubscriptionChargeResponse>();
    private SubscriptionPaymentResponse? subscriptionPaymentResponse;
    private bool canCancelateSubscription;

    public SubscriptionPaymentDetailViewModel(
        SubscriptionChargesService subscriptionChargesService,
        ISessionService sessionService,
        IDialogService dialogService,
        ITranslatorService translatorService,
        INavigationService navigationService,
        IClinicDetailService clinicDetailService,
        AppCenterAnalitics analitics)
    {
        Title = "Detalle de la subscripción";
        ShowBackButton = true;
        this.subscriptionChargesService = subscriptionChargesService;
        this.sessionService = sessionService;
        CancelSubscriptionCommand = new AsyncCommand(CancelSubscriptionCommandExecute);

        this.dialogService = dialogService;
        this.translatorService = translatorService;
        this.navigationService = navigationService;
        this.clinicDetailService = clinicDetailService;
        this.analitics = analitics;

        analitics.PageView(nameof(SubscriptionPaymentDetailViewModel));
    }

    public IAsyncCommand CancelSubscriptionCommand { get; set; }

    public bool CanCancelateSubscription { get => canCancelateSubscription; set => SetAndRaisePropertyChanged(ref canCancelateSubscription, value); }

    public List<SubscriptionChargeResponse> SubscriptionCharges
    {
        get => subscriptionCharges;
        set => SetAndRaisePropertyChanged(ref subscriptionCharges, value);
    }

    public SubscriptionPaymentResponse? SubscriptionPaymentResponse
    {
        get => subscriptionPaymentResponse;
        set => SetAndRaisePropertyChanged(ref subscriptionPaymentResponse, value);
    }

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
            CanCancelateSubscription = item.State == SubscriptionStates.ACTIVE;
            SubscriptionPaymentResponse = item;
            SubscriptionCharges = item.SubscriptionCharges.ToList();
        }
    }

    private async Task CancelSubscriptionCommandExecute()
    {
        await dialogService.ShowMessageYesNo(
                    translatorService.Translate("dialog_clinic_detail_cancel_subscription_clinic_title"),
                    translatorService.Translate("dialog_clinic_detail_cancel_subscription_clinic_body"),
        new AsyncCommand(async () =>
        {
            analitics.Clicked("Cancel subscription");
            IsBusy = true;
            var deleted = await clinicDetailService.CancelSubscription(SubscriptionPaymentResponse!.Id);
            IsBusy = false;
            if (deleted)
            {
                await dialogService.ShowMessage(
                                translatorService.Translate("dialog_clinic_detail_cancel_subscription_clinic_success_title"),
                                translatorService.Translate("dialog_clinic_detail_cancel_subscription_clinic_success_body"));
                await navigationService.BackAsync();
            }
        }));
    }
}
