namespace UploaderWasabi;

public static class WasabiUploaderServices
{
    public static IServiceCollection ConfigureWasabiUploaderServices(
        this IServiceCollection serviceProvider, 
        IConfiguration configuration)
    {

        string uri = configuration.BuildWasabiUri();

        var config = new AmazonS3Config 
        { 
            ServiceURL = uri,
        };


        var accesKey = configuration.GetRequiredSection("AWS:ACCESSKEY").Value;
        var secretKey = configuration.GetRequiredSection("AWS:SECRETKEY").Value;

        var credentials = new BasicAWSCredentials(accesKey, secretKey);

        var _s3Client = new AmazonS3Client(credentials, config) ;

        serviceProvider.AddSingleton<IAmazonS3>(_s3Client);

        serviceProvider.AddTransient<WasabiBucketService>();
        serviceProvider.AddTransient<WasabiUploadService>();
        serviceProvider.AddTransient<WasabiStorageService>();

        return serviceProvider;
    }
}
