using Microsoft.WindowsAPICodePack.Shell;
using System.Drawing;

namespace Freya.Services.UploadManager;

public class SessionHelper
{
    private WasabiStorageService azureUploaderService;
    private ILogger<SessionHelper> logger;
    private AppCenterService appCenterService;

    public SessionHelper(
        WasabiStorageService azureUploaderService,
        ILogger<SessionHelper> logger,
        AppCenterService appCenterService)
    {
        this.azureUploaderService = azureUploaderService;
        this.logger = logger;
        this.appCenterService = appCenterService;
    }

    public async Task<Uri?> TryGetAndUploadThumnail(Guid clientId, string path)
    {
        var jpgFileName = System.IO.Path.GetTempFileName() + ".jpg";
        Uri? uri = null;
        try
        {
            ShellFile shellFile = ShellFile.FromFilePath(path);
            Bitmap bm = shellFile.Thumbnail.Bitmap;
            bm.Save(jpgFileName, System.Drawing.Imaging.ImageFormat.Jpeg);

            var resultResponse = await azureUploaderService.Upload(clientId, jpgFileName);

            if (resultResponse.Success)
            {
                uri = resultResponse.Uri;
            }
        }
        catch (Exception e)
        {
            appCenterService.TrackError(e);
            logger.LogError(e.Message, e);
        }
        finally
        {
            try
            {
                if (File.Exists(jpgFileName))
                {
                    File.Delete(jpgFileName);
                }
            }
            catch (Exception e)
            {
                appCenterService.TrackError(e);
                logger.LogError(e.Message, e);
            }
        }

        return uri;
    }
}
