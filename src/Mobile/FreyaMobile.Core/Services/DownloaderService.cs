namespace FreyaMobile.Core.Services;

public class DownloaderService : IDownloaderService
{
    readonly HttpClient _httpClient = new HttpClient();
    private readonly ILogger<DownloaderService> logger;

    public DownloaderService(ILogger<DownloaderService> logger)
    {
        this.logger = logger;
    }

    public async Task<byte[]> DownloadFromUriAsync(Uri url, double timeout = 30000)
    {
        //_httpClient.Timeout = TimeSpan.FromSeconds(timeout);
        try
        {
            logger.LogInformation("Trying donwload from {file}", url);

            using var httpResponse = await _httpClient.GetAsync(url);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await httpResponse.Content.ReadAsByteArrayAsync();
            }
            else
            {
                //Url is Invalid
                return Array.Empty<byte>();
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error downloading from {uri}", url);
            return Array.Empty<byte>();
        }
    }
}
