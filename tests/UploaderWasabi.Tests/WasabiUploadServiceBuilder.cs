using Microsoft.Extensions.Configuration;
using Moq;

namespace UploaderWasabi.Tests;

internal class WasabiUploadServiceBuilder
{
    private static AmazonS3Client? _s3Client;
    private ILogger<WasabiUploadService> logger = new NullLogger<WasabiUploadService>();

    public WasabiUploadService Build()
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

        var configuration = new Mock<IConfiguration>();
        var configurationSectionRegion = new Mock<IConfigurationSection>();
        var configurationSectionUri = new Mock<IConfigurationSection>();
        configurationSectionRegion.Setup(x => x.Value).Returns(Consts.REGION);
        configurationSectionUri.Setup(x => x.Value).Returns(Consts.WASABI_URI);
        configuration.Setup(x => x.GetSection(It.Is<string>(k => k == "AWS:Region"))).Returns(configurationSectionRegion.Object);
        configuration.Setup(x => x.GetSection(It.Is<string>(k => k == "AWS:Uri"))).Returns(configurationSectionUri.Object);


        return new WasabiUploadService(_s3Client, logger, configuration.Object);
    }
}
