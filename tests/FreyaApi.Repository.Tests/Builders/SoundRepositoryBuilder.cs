namespace FreyaApi.Repository.Tests.Builders;

internal class SoundRepositoryBuilder
{
    public SoundRepository Build()
    {
        var dbContext = new DatabaseBuilder().Build();

        return new SoundRepository(dbContext);
    }
}
