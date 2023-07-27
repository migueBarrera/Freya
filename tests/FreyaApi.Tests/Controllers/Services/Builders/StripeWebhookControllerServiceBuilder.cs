namespace FreyaApi.Tests.Controllers.Services.Builders;

internal class StripeWebhookControllerServiceBuilder
{
    private ILogger<StripeWebhookControllerService> _logger = new NullLogger<StripeWebhookControllerService>();
    private Mock<ISubscriptionPaymentRepository> subscriptionPaymentRepository;
    private Mock<IPaymentSesionService> paymentSessionService;
    private Mock<ISubscriptionChargeRepository> subscriptionChargeRepository;

    public StripeWebhookControllerServiceBuilder()
    {
        subscriptionPaymentRepository = new Mock<ISubscriptionPaymentRepository>();
        paymentSessionService = new Mock<IPaymentSesionService>();
        subscriptionChargeRepository = new Mock<ISubscriptionChargeRepository>();
    }

    internal StripeWebhookControllerService Build()
    {
        return new StripeWebhookControllerService(
            _logger,
            paymentSessionService.Object,
            subscriptionPaymentRepository.Object,
            subscriptionChargeRepository.Object);
    }
}
