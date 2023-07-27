namespace UploaderWasabi.Tests;

internal class WasabiBucketServiceBuilder
{
    private static AmazonS3Client? _s3Client;
    private ILogger<WasabiBucketService> _logger = new NullLogger<WasabiBucketService>();

    public WasabiBucketService Build()
    {
        var config = new AmazonS3Config
        {
            ServiceURL = Consts.WASABI_URI_TEST,
        };

        // this will allow you to call whatever profile you have
        //var credentials = new StoredProfileAWSCredentials("sahanip");
        var credentials = new BasicAWSCredentials(Consts.ACCESSKEY, Consts.SECRETKEY);

        // create s3 connection with credential files and config.
        _s3Client = new AmazonS3Client(credentials, config);

        return new WasabiBucketService(_s3Client, _logger);
    }
}
