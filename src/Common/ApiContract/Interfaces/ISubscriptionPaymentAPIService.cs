using Models.Core.Subscriptions;

namespace ApiContract.Interfaces;

public interface ISubscriptionPaymentAPIService
{
    [Get("/api/SubscriptionPayment/v1/byclinic/{clinicId}")]
    Task<IEnumerable<SubscriptionPaymentResponse>> GetSubscriptionPaymentsByClinicId(Guid clinicId);
    
    [Get("/api/SubscriptionPayment/v1/{id}")]
    Task<SubscriptionPaymentResponse> GetSubscriptionPayment(Guid id);
    
    [Delete("/api/SubscriptionPayment/v1/{subscriptionId}")]
    Task<bool> CancelSubscription(Guid subscriptionId);
}
