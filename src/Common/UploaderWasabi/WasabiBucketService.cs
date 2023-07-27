namespace UploaderWasabi;

public class WasabiBucketService
{
    private readonly IAmazonS3 _s3Client;
    private readonly ILogger<WasabiBucketService> logger;

    public WasabiBucketService(
        IAmazonS3 s3Client, 
        ILogger<WasabiBucketService> logger)
    {
        _s3Client = s3Client;
        this.logger = logger;
    }

    public async Task Delete(string bucket)
    {
        logger.LogInformation("Trying delete a bucket {0}", bucket);
        await _s3Client.DeleteBucketAsync(bucket);
    }

    public async Task EnsureCreateBucket(string bucketPath)
    {
        var paths = bucketPath.Split('/');
        logger.LogInformation("Ensuring create a bucket {0}", bucketPath);
        logger.LogInformation("Bucket paths {0}", paths);
        var bucketName = paths[0];
        if (await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName))
        {
            logger.LogInformation("Not necessary create bucket {0}", bucketName);
            logger.LogInformation("Ensure bucket path exists {0}", bucketPath);
            await _s3Client.EnsureBucketExistsAsync(bucketPath);
        }
        else
        {
            logger.LogInformation("Creating new bucket, bucketName {0}", bucketName);
            logger.LogInformation("Creating new bucket, bucketPath {0}", bucketPath);
            await _s3Client.PutBucketAsync(bucketName);
            await _s3Client.PutBucketAsync(bucketPath);
        }
    }
}
