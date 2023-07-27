namespace FreyaManager.Features.Settings;

public class SettingViewModel : CoreViewModel
{
    private readonly IAppUpdaterManagerService appUpdaterService;
    private readonly SettingsService settingsService;

    public SettingViewModel(
        IAppUpdaterManagerService appUpdaterService,
        SettingsService settingsService,
        AppVersionService appVersionService,
        IConfiguration configuration)
    {
        Title = "Ajustes";
        this.appUpdaterService = appUpdaterService;

        CheckUpdateCommand = new AsyncCommand(CheckUpdateCommandExecute);
        ResetCommand = new AsyncCommand(ResetCommandExecute);
        ApiInfoCommand = new AsyncCommand(ApiInfoCommandExecute);
        this.settingsService = settingsService;

        bool isProductionEvironment = configuration.GetValue<bool>("IsProductionEvironment");
        string textDevelopEvironment = isProductionEvironment ? string.Empty : "Dev";

        var version = appVersionService.GetAppVersion();
        Version = $"{version.Major}.{version.Minor}.{version.Build} v {textDevelopEvironment}";
    }

    public IAsyncCommand CheckUpdateCommand { get; set; }

    public IAsyncCommand ResetCommand { get; set; }

    public IAsyncCommand ApiInfoCommand { get; set; }

    public string Version { get; set; } = string.Empty;

    private async Task CheckUpdateCommandExecute()
    {
        await appUpdaterService.CheckAndTryUpdate(showErrorDialog: true);
    }
    
    private async Task ResetCommandExecute()
    {
        await settingsService.ResetDatabase();
    }

    private async Task ApiInfoCommandExecute()
    {
        await settingsService.GetApiInfo();
    }
}
