namespace FreyaApi.Repository.Tests.Builders;

internal class SubscriptionPaymentRepositoryBuilder
{
    public DatabaseContext? dbContext { get; private set; }

    public SubscriptionPaymentRepository Build()
    {
        dbContext = new DatabaseBuilder().Build();

        return new SubscriptionPaymentRepository(dbContext);
    }
}
