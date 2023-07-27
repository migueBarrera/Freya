namespace UploaderWasabi;

public class WasabiStorageService
{
    private readonly WasabiUploadService uploadService;
    private readonly WasabiBucketService wasabiBucketService;
    private readonly ILogger<WasabiStorageService> logger;

    public WasabiStorageService(
        WasabiUploadService uploadService,
        WasabiBucketService wasabiBucketService,
        ILogger<WasabiStorageService> logger)
    {
        this.uploadService = uploadService;
        this.wasabiBucketService = wasabiBucketService;
        this.logger = logger;
    }

    public async Task<UploadObjectResponse> Upload(Guid clientId, string localFilePath)
    {
        logger.LogInformation("Trying upload a item for a client {0}, from path {1}", clientId, localFilePath);
        string bucketName = "client" + clientId.ToString();

        await wasabiBucketService.EnsureCreateBucket(bucketName);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(localFilePath)}";
        logger.LogInformation("Generate new file name {0}", fileName);

        var response = await uploadService.UploadObjectFromFile(bucketName, fileName, localFilePath);

        return response;
    }
}
