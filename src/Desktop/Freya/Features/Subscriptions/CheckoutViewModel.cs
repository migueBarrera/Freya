namespace Freya.Features.Subscriptions;

public class CheckoutViewModel : CoreViewModel
{
    private readonly ISessionService sessionService;
    private readonly ICurrentClinicService currentClinicService;
    private readonly INavigationService navigationService;
    private readonly ICurrentEmployeeService currentEmployeeService;
    private readonly AppCenterAnalitics appCenterService;
    private Uri source = new Uri("about:blank");

    public CheckoutViewModel(
        ISessionService sessionService,
        ICurrentClinicService currentClinicService,
        ICurrentEmployeeService currentEmployeeService,
        INavigationService navigationService,
        AppCenterAnalitics appCenterService)
    {
        ShowBackButton = true;
        this.sessionService = sessionService;
        this.currentClinicService = currentClinicService;
        this.currentEmployeeService = currentEmployeeService;
        this.navigationService = navigationService;
        this.appCenterService = appCenterService;

        appCenterService.PageView(nameof(CheckoutPage));
    }

    public override Task OnAppearingAsync(object? parameter = null)
    {
        var employee = currentEmployeeService.Employee;
        var clinic = currentClinicService.CurrentClinic;
        var subscriptionPlan = sessionService.Get<PaymentsPlansForCompanyResponse>(SESSION.KEY_SUBSCRIPTION_SELECTED);
        if (employee == null || clinic == null || subscriptionPlan == null)
        {
            return navigationService.BackAsync();
        }

        Source = new Uri($"{subscriptionPlan.PaymentLinkMonthly}?client_reference_id={clinic?.Id}&prefilled_email={employee?.Email}");
        return base.OnAppearingAsync();
    }

    public Uri Source { get => source; set => SetAndRaisePropertyChanged(ref source, value); }
}
