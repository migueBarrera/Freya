using Stripe;

namespace FreyaPayments.Core;

public interface IPaymentSesionService
{
    Task<PaymentStripeModel?> GetSubscriptionPlanIdFromSessionId(string sesionId);

    Task<PaymentStripeModel?> GetSubscriptionPlanIdFromSessionIdAlternative(Invoice paymentIntent);

    Task<bool> CancelSubscriptionNow(string stripeSubscriptionId);
}

public class PaymentStripeModel
{
    public Guid ClinicId { get; set; }
    public Guid SubscriptionPlainId { get; set; }
    public string StripeCustomerId { get; set; } = string.Empty;
    public string StripeSubcriptionId { get; set; } = string.Empty;
    public bool IsMonthly { get; set; } = false;

    public bool IsAnnual { get; set; } = false;
}
