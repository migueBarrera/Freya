namespace FreyaApi.Repository.Tests.Builders;

internal class ImageRepositoryBuilder
{
    public ImageRepository Build()
    {
        var dbContext = new DatabaseBuilder().Build();

        return new ImageRepository(dbContext);
    }
}
