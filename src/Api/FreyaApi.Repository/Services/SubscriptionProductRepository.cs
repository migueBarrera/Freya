namespace FreyaApi.Repository.Services;

public class SubscriptionProductRepository : BaseRepository, ISubscriptionProductRepository
{
    public SubscriptionProductRepository(DatabaseContext databaseContext)
         : base(databaseContext)
    {
    }

    public async Task<bool> CreateSubscriptionProducts(List<SubscriptionProductTable> products)
    {
        context.AddRange(products);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CreateSubscriptionProducts(SubscriptionProductTable product)
    {
        context.Add(product);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<SubscriptionProductTable>> GetAllSubscriptionProducts()
    {
        var items = await context?
                .SubscriptionProducts?
                .ToListAsync()!;

        return items ?? Enumerable.Empty<SubscriptionProductTable>();
    }

    public async Task<IEnumerable<SubscriptionProductTable>> GetSubscriptionProductsByIds(IEnumerable<Guid> subscriptionIds)
    {
        var subscriptionsProducts = await context.SubscriptionProducts?
        .Where(sp => sp.IsActive && subscriptionIds.Contains(sp.Id))
        .ToListAsync()!;

        return subscriptionsProducts;
    }
}
