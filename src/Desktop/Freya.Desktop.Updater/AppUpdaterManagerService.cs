using Microsoft.Extensions.DependencyInjection;

namespace Freya.Desktop.Updater;

public class AppUpdaterManagerService : IAppUpdaterManagerService
{
    private readonly AppUpdaterService appUpdaterService;
    private readonly IDialogService dialogService;
    private readonly ITranslatorService translatorService;
    private readonly IServiceProvider serviceProvider;

    public AppUpdaterManagerService(
        AppUpdaterService appUpdaterService,
        IDialogService dialogService,
        ITranslatorService translatorService,
        IServiceProvider serviceCollection)
    {
        this.appUpdaterService = appUpdaterService;
        this.dialogService = dialogService;
        this.translatorService = translatorService;
        this.serviceProvider = serviceCollection;
    }


    public async Task<bool> CheckAndTryUpdate(bool showErrorDialog = false)
    {
        if (await appUpdaterService.IsThereAnUpdate())
        {
            await dialogService.ShowMessageYesNo(
                        translatorService.Translate("dialog_updater_update_available_title"),
                        translatorService.Translate("dialog_updater_update_available_body"),
                        new AsyncCommand(() => UserWhantUpdate()));
            return true;
        }
        else
        {
            if (showErrorDialog)
            {
                await dialogService.ShowMessage(
                        translatorService.Translate("dialog_updater_update_not_available_title"),
                        translatorService.Translate("dialog_updater_update_not_available_body"));
            }
            return false;
        }
    }

    private async Task UserWhantUpdate()
    {
        var dialog = serviceProvider.GetRequiredService<UploadingDialog>();
        if (dialog != null)
        {
            await dialogService.ShowDialog(dialog);
        }
    }
}
