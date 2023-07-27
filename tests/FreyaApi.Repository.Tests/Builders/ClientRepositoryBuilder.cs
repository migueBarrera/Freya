namespace FreyaApi.Repository.Tests.Builders;

internal class ClientRepositoryBuilder
{
    public DatabaseContext? dbContext { get; private set; }

    public ClientRepository Build()
    {
        dbContext = new DatabaseBuilder().Build();

        return new ClientRepository(dbContext);
    }

}
