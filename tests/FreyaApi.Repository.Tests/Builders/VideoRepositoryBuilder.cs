namespace FreyaApi.Repository.Tests.Builders;

internal class VideoRepositoryBuilder
{
    public VideoRepository Build()
    {
        var dbContext = new DatabaseBuilder().Build();

        return new VideoRepository(dbContext);
    }
}
