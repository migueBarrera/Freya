namespace FreyaApi.Repository.Interfaces;

public interface ISubscriptionProductRepository
{
    Task<bool> CreateSubscriptionProducts(List<SubscriptionProductTable> products);

    Task<bool> CreateSubscriptionProducts(SubscriptionProductTable product);

    Task<IEnumerable<SubscriptionProductTable>> GetAllSubscriptionProducts();

    Task<IEnumerable<SubscriptionProductTable>> GetSubscriptionProductsByIds(IEnumerable<Guid> subscriptionIds);
}
