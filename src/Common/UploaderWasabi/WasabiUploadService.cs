namespace UploaderWasabi;

public class WasabiUploadService
{
    private IAmazonS3 _s3Client;
    private readonly ILogger<WasabiUploadService> logger;
    private readonly IConfiguration configuration;

    public WasabiUploadService(
        IAmazonS3 s3Client, 
        ILogger<WasabiUploadService> logger,
        IConfiguration configuration)
    {
        _s3Client = s3Client;
        this.logger = logger;
        this.configuration = configuration;
    }

    public async Task<bool> DeleteFile(string bucketName, string fileKey)
    {
        try
        {
            await _s3Client.DeleteAsync(bucketName, fileKey, null);
            return true;
        }
        catch (AmazonS3Exception e)
        {
            logger.LogError(e, e.Message);
            return false;
        }
    }

    public async Task<bool> RemoveRange(string bucketName, string[] fileNameOnBlob)
    {
        try
        {
            await _s3Client.DeletesAsync(bucketName, fileNameOnBlob.AsEnumerable(), null);
            return true;
        }
        catch (AmazonS3Exception e)
        {
            logger.LogError(e, e.Message);
            return false;
        }
    }

    public async Task<UploadObjectResponse> UploadObjectFromFile(
        string bucketName,
        string fileNameOnBucket,
        string filePath)
    {
        var response = new UploadObjectResponse();

        logger.LogInformation(
            "Trying put object to bucket {0} , with file name on bucket {1}, from original file {2}", 
            bucketName, 
            fileNameOnBucket, 
            filePath);
        try
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = fileNameOnBucket,
                FilePath = filePath,
                CannedACL = new S3CannedACL("public-read"),
            };

            var responsePut = await _s3Client.PutObjectAsync(putRequest);
            if (responsePut.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                var url = configuration.BuildUploadWasabiUri(bucketName, fileNameOnBucket);
                response.Success = true ;
                response.Uri = new Uri(url);
            }
        }
        catch (AmazonS3Exception e)
        {
            logger.LogError(e, e.Message);
        }

        return response;
    }
}