namespace FreyaApi.Repository.Tests.Builders;

internal class SubscriptionPlanRepositoryBuilder
{
    public SubscriptionPlanRepository Build()
    {
        var dbContext = new DatabaseBuilder().Build();

        return new SubscriptionPlanRepository(dbContext);
    }
}
