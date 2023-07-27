using Android.Content;
using FreyaMobile.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FreyaMobile.Platforms;

internal class NativeMediaService : INativeMediaService
{
    private readonly ILogger<NativeMediaService> logger;

    public NativeMediaService(ILogger<NativeMediaService> logger)
    {
        this.logger = logger;
    }

    public bool SaveImageFromByte(byte[] imageByte, string filename)
    {
        Java.IO.File? storagePath;

        try
        {
            storagePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads ?? string.Empty);

            if (storagePath == null)
            {
                return false;
            }

            string path = System.IO.Path.Combine(storagePath.ToString(), filename);
            System.IO.File.WriteAllBytes(path, imageByte);
            var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            mediaScanIntent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(path)));
            Android.App.Application.Context.SendBroadcast(mediaScanIntent);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return false;
        }
    }
}
