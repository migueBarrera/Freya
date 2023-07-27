namespace UploaderWasabi.Tests;


[TestFixture]
public class WasabiBucketServiceShould
{
    private const string BUCKET_NAME = "testing-backet";
    private WasabiBucketServiceBuilder builder;

    [SetUp]
    public void Setup()
    {
        builder = new WasabiBucketServiceBuilder();
    }

    [Test]
    public async Task CanCreateAndDeleteABasicBucket()
    {
        var service = builder.Build();

        await service.EnsureCreateBucket(BUCKET_NAME);
        await service.Delete(BUCKET_NAME);
    }

    [Test]
    public async Task CanCreateAndDeleteAPathBucket()
    {
        var service = builder.Build();

        var bucketPath = WasabiBucketFolderHelper.BuildFolder(BUCKET_NAME, "test");
        await service.EnsureCreateBucket(bucketPath);
        await service.Delete(bucketPath);
        await service.Delete(BUCKET_NAME);
    }
}

