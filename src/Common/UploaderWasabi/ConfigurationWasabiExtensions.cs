namespace UploaderWasabi;

internal static class ConfigurationWasabiExtensions
{
    internal static string BuildWasabiUri(this IConfiguration configuration)
    {
        var region = configuration.GetRequiredSection("AWS:Region").Value;
        var uriNoFormat = configuration.GetRequiredSection("AWS:Uri").Value;

        if (uriNoFormat == null)
        {
            return string.Empty;
        }

        return string.Format(uriNoFormat, region);
    }
    
    
    internal static string BuildUploadWasabiUri(this IConfiguration configuration, string bucketName, string fileNameOnBucket)
    {
        var uri = configuration.BuildWasabiUri();
        if (string.IsNullOrEmpty(uri))
        {
            return string.Empty;
        }

        return $"{uri}{bucketName}/{fileNameOnBucket}";
    }
}
