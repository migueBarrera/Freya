namespace FreyaApi.Repository.Tests.Builders;

internal class SubscriptionProductRepositoryBuilder
{
    public SubscriptionProductRepository Build()
    {
        var dbContext = new DatabaseBuilder().Build();

        return new SubscriptionProductRepository(dbContext);
    }
}
