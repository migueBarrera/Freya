namespace Freya.Features.Settings;

public class SettingViewModel : CoreViewModel
{
    private readonly IAppUpdaterManagerService appUpdaterService;
    private readonly AppCenterAnalitics appCenterAnalitics;

    public SettingViewModel(
        IAppUpdaterManagerService appUpdaterService,
        ITranslatorService translatorService,
        AppCenterAnalitics appCenterAnalitics)
    {
        Title = translatorService.Translate("page_title_settings");

        CheckUpdateCommand = new AsyncCommand(CheckUpdateCommandExecute);
        this.appUpdaterService = appUpdaterService;

        this.appCenterAnalitics = appCenterAnalitics;

        appCenterAnalitics.PageView(nameof(SettingViewModel));
    }

    public IAsyncCommand CheckUpdateCommand { get; set; }

    private async Task CheckUpdateCommandExecute()
    {
        await appUpdaterService.CheckAndTryUpdate(showErrorDialog: true);
    }
}
