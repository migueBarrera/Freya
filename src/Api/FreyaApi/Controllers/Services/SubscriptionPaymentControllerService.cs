namespace FreyaApi.Controllers.Services;

public class SubscriptionPaymentControllerService
{
    private readonly IPaymentSesionService paymentSesionService;
    private readonly ISubscriptionPaymentRepository subscriptionPaymentRepository;

    public SubscriptionPaymentControllerService(
        IPaymentSesionService paymentSesionService,
        ISubscriptionPaymentRepository subscriptionPaymentRepository)
    {
        this.paymentSesionService = paymentSesionService;
        this.subscriptionPaymentRepository = subscriptionPaymentRepository;
    }

    public async Task<ActionResult<IEnumerable<SubscriptionPaymentResponse>>> GetSubscriptionPayments(ControllerBase controller, Guid clinicId)
    {
        var paymentPlans = await subscriptionPaymentRepository.GetPaymentsByClinic(clinicId);

        var response = paymentPlans.Select(x => SubscriptionsMapper.ConverTo(x));

        return controller.Ok(response);
    }

    public async Task<ActionResult<bool>> CancelSubscription(ControllerBase controller, Guid subscriptionId)
    {
        var paymentPlan = await subscriptionPaymentRepository.GetSubscriptionPayment(subscriptionId);

        if (paymentPlan == null)
        {
            return controller.NotFound();
        }

        var cancelOnStripe = await paymentSesionService.CancelSubscriptionNow(paymentPlan.StripeSubscriptionId);
        if (!cancelOnStripe)
        {
            return controller.NotFound();
        }

        await subscriptionPaymentRepository.UpdateState(subscriptionId, SubscriptionStates.REJECTED_BY_EMPLOYEE);


        return controller.Ok(true);
    }

    internal async Task<ActionResult<SubscriptionPaymentResponse>> GetSubscriptionPayment(SubscriptionPaymentController controller, Guid id)
    {
        var paymentPlan = await subscriptionPaymentRepository.GetSubscriptionPayment(id);
        if (paymentPlan == null)
        {
            return controller.NotFound();
        }

        var response = SubscriptionsMapper.ConverTo(paymentPlan);

        return controller.Ok(response);
    }
}
