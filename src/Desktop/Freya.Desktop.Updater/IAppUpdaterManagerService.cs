namespace Freya.Desktop.Updater;

public interface IAppUpdaterManagerService
{
    Task<bool> CheckAndTryUpdate(bool showErrorDialog = false);
}
