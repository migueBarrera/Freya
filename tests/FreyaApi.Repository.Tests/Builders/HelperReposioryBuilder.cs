namespace FreyaApi.Repository.Tests.Builders;

internal class HelperReposioryBuilder
{
    public HelperReposiory Build()
    {
        var dbContext = new DatabaseBuilder().Build();

        return new HelperReposiory(dbContext);
    }
}
