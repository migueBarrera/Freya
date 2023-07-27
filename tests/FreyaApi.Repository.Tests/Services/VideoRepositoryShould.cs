namespace FreyaApi.Repository.Tests.Services;

[TestFixture]
internal class VideoRepositoryShould
{
    private VideoRepositoryBuilder builder;

    [SetUp]
    public void SetUp()
    {
        builder = new VideoRepositoryBuilder();
    }
}
