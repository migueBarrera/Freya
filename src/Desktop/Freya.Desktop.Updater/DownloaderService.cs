using System.IO;
using System.Net.Http;

namespace Freya.Desktop.Updater;

public class DownloaderService
{
    public async Task<string> Download(HttpClient http,string updateUrl, Progress<float> progress)
    {
        var timeoutCancellationToken = new CancellationTokenSource();
        timeoutCancellationToken.CancelAfter(TimeSpan.FromSeconds(120));

        var tempUpdateFile = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
        using var fileStremUploaded = new FileStream(tempUpdateFile, FileMode.Create, FileAccess.Write, FileShare.None);

        await http.DownloadAsync(updateUrl, fileStremUploaded, progress, timeoutCancellationToken.Token);

        return fileStremUploaded.Name;
    }
}
