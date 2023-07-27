using MvvmHelpers;

namespace Freya.Desktop.Updater;

public class UploadingDialogViewModel : BaseViewModel
{
    private readonly AppUpdaterService appUpdaterService;
    private readonly IDialogService dialogService;
    private readonly ITranslatorService translatorService;

    private float progress;

    public UploadingDialogViewModel(AppUpdaterService appUpdaterService, ITranslatorService translatorService, IDialogService dialogService)
    {
        this.appUpdaterService = appUpdaterService;
        this.translatorService = translatorService;
        this.dialogService = dialogService;
    }

    public float Progress { get => progress; set => SetProperty(ref progress, value); }

    internal async Task Initialize()
    {
        int lastProgressUpdate = 0;
        Progress<float> progress = new();
        progress.ProgressChanged += (object? sender, float value) =>
        {
            var valueTruc = (int)(100 * value);
            if (valueTruc - lastProgressUpdate >= 2)
            {
                Progress = valueTruc;
                lastProgressUpdate = valueTruc;
            }
        };

        var isUpdated = await appUpdaterService.Update(progress);

        if (isUpdated)
        {
            await dialogService.ShowMessage(
                        translatorService.Translate("dialog_updater_update_success_title"),
                        translatorService.Translate("dialog_updater_update_success_body"));
        }
        else
        {
            await dialogService.ShowMessage(
                        translatorService.Translate("dialog_updater_update_fail_title"),
                        translatorService.Translate("dialog_updater_update_fail_body"));
        }
    }
}
