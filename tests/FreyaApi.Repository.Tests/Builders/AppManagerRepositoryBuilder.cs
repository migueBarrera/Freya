namespace FreyaApi.Repository.Tests.Builders;

internal class AppManagerRepositoryBuilder
{
    public AppManagerRepository Build()
    {
        var dbContext = new DatabaseBuilder().Build();

        return new AppManagerRepository(dbContext);
    }
}
