using ApiContract.Interfaces;

namespace FreyaManager.Features.Settings;

public class SettingsService
{
    private readonly ILogger<SettingsService> logger;
    private readonly ITaskHelperFactory taskHelperFactory;
    private readonly IDialogService dialogService;
    private readonly IHelperAPIService helperAPIService;

    public SettingsService(
        IRefitService refitService,
        ILogger<SettingsService> logger,
        ITaskHelperFactory taskHelperFactory,
        IDialogService dialogService)
    {
        helperAPIService = refitService.InitRefitInstance<IHelperAPIService>(isAutenticated: false);
        this.logger = logger;
        this.taskHelperFactory = taskHelperFactory;
        this.dialogService = dialogService;
    }

    public async Task<bool> GetApiInfo()
    {
        var result = await taskHelperFactory.
                             CreateInternetAccessViewModelInstance(logger).
                             WithErrorHandling(OnSignInError).
                             TryExecuteAsync(
                             () => helperAPIService.GetApiInfo());

        if (result.IsSuccess)
        {
            await dialogService.ShowMessage("Información de api", result.Value?.ToString()!);
        }

        return result.IsSuccess;
    }
    
    public async Task<bool> ResetDatabase()
    {
        await dialogService.ShowMessageYesNo(
            "Cuidado", 
            "¿Estas seguro que quieres resetear toda la base de datos?", 
            new AsyncCommand(UserWantReset));
        
        return true;
    }

    private async Task UserWantReset()
    {
        var result = await taskHelperFactory.
                             CreateInternetAccessViewModelInstance(logger).
                             WithErrorHandling(OnSignInError).
                             TryExecuteAsync(
                             () => helperAPIService.ResetAllDatabase(true));

        if (result.IsSuccess)
        {
            await dialogService.ShowMessage("Datos reset", "La base de datos se ha recreado");
        }
    }

    private Task<bool> OnSignInError(Exception arg)
    {
        return Task.FromResult(false);
    }
}
