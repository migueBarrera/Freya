namespace FreyaMobile.Core.Services.Interfaces;

public interface IDownloaderService
{
    Task<byte[]> DownloadFromUriAsync(Uri url, double timeout = 30000);
}
