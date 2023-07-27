using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;

namespace FreyaPayments.Core;

public class PaymentSesionService : IPaymentSesionService
{
    private ILogger<PaymentSesionService> logger;

    public PaymentSesionService(ILogger<PaymentSesionService> logger)
    {
        this.logger = logger;
    }

    public Task<bool> CancelSubscriptionNow(string stripeSubscriptionId)
    {
        var service = new SubscriptionService();
        var options = new SubscriptionUpdateOptions
        {
            CancelAt = DateTime.Now.AddHours(1),
            ProrationBehavior = "none",
        };

        try
        {
            Subscription subscription = service.Update(stripeSubscriptionId, options);
        }
        catch (StripeException e)
        {
            bool subscriptionCanceled = e.Message?.Contains("You cannot update a subscription that is `canceled` or `incomplete_expired`.") ?? false;
            if (!subscriptionCanceled)
            {
                logger.LogError(e, e.Message);
                return Task.FromResult(false);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Task.FromResult(false);
        }

        return Task.FromResult(true);
    }

    public Task<PaymentStripeModel?> GetSubscriptionPlanIdFromSessionId(string sesionId)
    {
        PaymentStripeModel? payment = null;

        try
        {
            var sessionService = new SessionService();
            Session session = sessionService.Get(sesionId);

            var customerService = new CustomerService();
            Customer customer = customerService.Get(session.CustomerId);
            session.Metadata.TryGetValue("subscriptionPlanId", out var subscriptionPlanId);

            if (subscriptionPlanId == null)
            {
                logger.LogError("Can not read a SubscriptionPlanId on session, discarting...", session);
                return Task.FromResult(payment);
            }

            payment = new PaymentStripeModel();
            payment.SubscriptionPlainId = Guid.Parse(subscriptionPlanId);
            payment.ClinicId = Guid.Parse(session.ClientReferenceId);
            payment.StripeCustomerId = customer.Id;
            payment.StripeSubcriptionId = session.SubscriptionId;
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }


        return Task.FromResult(payment);
    }

    public Task<PaymentStripeModel?> GetSubscriptionPlanIdFromSessionIdAlternative(Invoice paymentIntent)
    {
        PaymentStripeModel? payment = null;

        try
        {
            logger.LogWarning($"Analisysn: {paymentIntent}");
            var options = new SessionListOptions
            {
                Limit = 1,
                Customer = paymentIntent.CustomerId,
            };

            var service = new SessionService();
            StripeList<Session> sessions = service.List(
              options);

            if (sessions.Count() == 0)
            {
                logger.LogWarning($"Can not found any session with CustomerId: {paymentIntent.CustomerId}");
                return Task.FromResult(payment);
            }

            Session session = sessions.First();

            session.Metadata.TryGetValue("subscriptionPlanId", out var subscriptionPlanId);
            session.Metadata.TryGetValue("isMonthly", out var isMonthly);
            session.Metadata.TryGetValue("isAnnual", out var isAnnual);

            payment = new PaymentStripeModel();
            payment.SubscriptionPlainId = Guid.Parse(subscriptionPlanId!);
            payment.ClinicId = Guid.Parse(session.ClientReferenceId);
            payment.StripeCustomerId = paymentIntent.CustomerId;
            payment.StripeSubcriptionId = session.SubscriptionId;
            payment.IsAnnual = bool.Parse(isAnnual ?? "false");
            payment.IsMonthly = bool.Parse(isMonthly ?? "false");
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }

        return Task.FromResult(payment);
    }
}
