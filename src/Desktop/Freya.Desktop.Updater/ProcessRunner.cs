using AppCenterServices;

namespace Freya.Desktop.Updater;

public class ProcessRunner
{
    private readonly ILogger<ProcessRunner> logger;
    private AppCenterService appCenterService;

    public ProcessRunner(
        ILogger<ProcessRunner> logger, 
        AppCenterService appCenterService)
    {
        this.logger = logger;
        this.appCenterService = appCenterService;
    }

    public bool Run(string fullpath, bool makeSilent)
    {
        try
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fullpath,
                    CreateNoWindow = true,
                    UseShellExecute = true,
                    Arguments = makeSilent 
                        ?"/S"
                        : string.Empty,
                },
            };

            process.Start();

            return true;
        }
        catch (Win32Exception nativeException)
        {
            /// 1223 Native error code is when user cancels de run of the process. 
            /// So we only mark que operation as not succeeded when error code is different from 1223
            if (nativeException.NativeErrorCode == 1223)
            {
                logger.LogWarning("User cancel task...");
                return false;
            }
            else
            {
                appCenterService.TrackError(nativeException);
                logger.LogError(nativeException.Message);

            }
            return false;
        }
        catch (Exception ex)
        {
            appCenterService.TrackError(ex);
            logger.LogError(ex.Message);
            return false;
        }
    }
}
