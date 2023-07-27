using System.Diagnostics.CodeAnalysis;

namespace UploaderWasabi.Tests;

[TestFixture]
public class WasabiUploadServiceShould
{
    private const string BUCKET_NAME = "testing-backet";
    private WasabiBucketServiceBuilder wasabiBucketServiceBuilder;
    private WasabiUploadServiceBuilder wasabiUploadServiceBuilder;

    [SetUp]
    public void Setup()
    {
        wasabiBucketServiceBuilder = new WasabiBucketServiceBuilder();
        wasabiUploadServiceBuilder = new WasabiUploadServiceBuilder();
    }

    [Test]
    public async Task CanUploadAndDeleteAFileWithComplexPath()
    {
        var wasabiBucketService = wasabiBucketServiceBuilder.Build();
        var wasabiUploaderService = wasabiUploadServiceBuilder.Build();

        var bucketPath = WasabiBucketFolderHelper.BuildFolder(BUCKET_NAME, "test");
        await wasabiBucketService.EnsureCreateBucket(bucketPath);


        string appFolderPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
        var filePath = Directory.GetFiles(appFolderPath)
            .First(z => z.EndsWith("testimage.png"));
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(filePath)}";
        await wasabiUploaderService.UploadObjectFromFile(bucketPath, fileName, filePath);

        var fileDeleted = await wasabiUploaderService.DeleteFile(bucketPath, fileName);
        if (fileDeleted)
        {
            await wasabiBucketService.Delete(bucketPath);
            await Task.Delay(200);
            await wasabiBucketService.Delete(BUCKET_NAME);
        }
        else
        {
            Assert.Fail("Can not delete file on bucket");
        }
    }

    [Test]
    public async Task CanUploadAndDeleteAFile()
    {
        var wasabiBucketService = wasabiBucketServiceBuilder.Build();
        var wasabiUploaderService = wasabiUploadServiceBuilder.Build();

        await wasabiBucketService.EnsureCreateBucket(BUCKET_NAME);

        string appFolderPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
        var filePath = Directory.GetFiles(appFolderPath)
            .First(z => z.EndsWith("testimage.png"));
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(filePath)}";
        var responseUpload = await wasabiUploaderService.UploadObjectFromFile(BUCKET_NAME, fileName, filePath);

        Assert.That(responseUpload.Success, Is.True);
        Assert.That(Uri.IsWellFormedUriString(responseUpload.Uri!.ToString(), UriKind.Absolute), Is.True);

        var fileDeleted = await wasabiUploaderService.DeleteFile(BUCKET_NAME, fileName);
        if (fileDeleted)
        {
            await wasabiBucketService.Delete(BUCKET_NAME);
        }
        else
        {
            Assert.Fail("Can not delete file on bucket");
        }
    }

    [Test]
    public async Task CanUploadAndDeleteAMultipleFile()
    {
        var wasabiBucketService = wasabiBucketServiceBuilder.Build();
        var wasabiUploaderService = wasabiUploadServiceBuilder.Build();

        await wasabiBucketService.EnsureCreateBucket(BUCKET_NAME);

        string appFolderPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
        var filePath = Directory.GetFiles(appFolderPath)
            .First(z => z.EndsWith("testimage.png"));

        var files = Enumerable.Range(1, 10).Select(x => $"{Guid.NewGuid()}{Path.GetExtension(filePath)}").ToArray();
        foreach (var file in files)
        {
            var responseUpload = await wasabiUploaderService.UploadObjectFromFile(BUCKET_NAME, file, filePath);
        }

        var fileDeleted = await wasabiUploaderService.RemoveRange(BUCKET_NAME, files);
        if (fileDeleted)
        {
            await wasabiBucketService.Delete(BUCKET_NAME);
            Assert.Pass();
        }
        else
        {
            Assert.Fail("Can not delete file on bucket");
        }
    }

    [Test]
    [Ignore("only for test propouses, because not delete uploaded file")]
    [ExcludeFromCodeCoverage]
    public async Task UploadFile()
    {
        var wasabiBucketService = wasabiBucketServiceBuilder.Build();
        var wasabiUploaderService = wasabiUploadServiceBuilder.Build();

        await wasabiBucketService.EnsureCreateBucket(BUCKET_NAME);

        string appFolderPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
        var filePath = Directory.GetFiles(appFolderPath)
            .First(z => z.EndsWith("testimage.png"));
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(filePath)}";
        var response = await wasabiUploaderService.UploadObjectFromFile(BUCKET_NAME, fileName, filePath);

        Assert.That(response.Success, Is.True);
        Assert.That(Uri.IsWellFormedUriString(response.Uri!.ToString(), UriKind.Absolute), Is.True);
    }
}
