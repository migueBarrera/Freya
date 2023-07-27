namespace FreyaApi.Repository.Interfaces;

public interface ISubscriptionPaymentRepository
{
    Task<SubscriptionPaymentTable?> GetCurrentActiveClinicSubscription(Guid clinicId);

    Task<SubscriptionPaymentTable?> GetSubscriptionByStripeCustomerId(string stripeCustomerId);

    Task<SubscriptionPaymentTable?> GetSubscriptionPayment(Guid subscriptioPaymentId);

    Task<IEnumerable<SubscriptionPaymentTable>> GetPaymentsByClinic(Guid id);

    Task<bool> UpdateState(Guid subscriptionId, SubscriptionStates newState);

    Task<bool> Create(SubscriptionPaymentTable subscription);
}
