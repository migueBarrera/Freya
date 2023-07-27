using Models.Core.Subscriptions;

namespace ApiContract.Interfaces;

public interface ISubscriptionProductAPIService
{
    [Post("/api/SubscriptionProduct/v1")]
    Task CreateAsync([Body] SubscriptionProductCreateRequest request);

    [Get("/api/SubscriptionProduct/v1")]
    Task<IEnumerable<SubscriptionProductResponse>> GetSubscriptionProducts();
}
