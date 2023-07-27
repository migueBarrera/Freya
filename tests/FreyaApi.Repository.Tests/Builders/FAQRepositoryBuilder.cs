namespace FreyaApi.Repository.Tests.Builders;

internal class FAQRepositoryBuilder
{
    public FAQRepository Build()
    {
        var dbContext = new DatabaseBuilder().Build();

        return new FAQRepository(dbContext);
    }
}
