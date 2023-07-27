namespace Freya.Desktop.Core.Services;

public class TaskHelperFactory : ITaskHelperFactory
{
    private readonly IDialogService dialogsService;
    private readonly IConnectivityService connectivityService;
    private readonly ITranslatorService translatorService;
    private readonly AppCenterService appCenterService;

    public TaskHelperFactory(
        IDialogService dialogsService,
        IConnectivityService connectivityService,
        ITranslatorService translatorService,
        AppCenterService appCenterService)
    {
        this.dialogsService = dialogsService;
        this.connectivityService = connectivityService;
        this.translatorService = translatorService;
        this.appCenterService = appCenterService;
    }

    public ITaskHelper CreateInternetAccessViewModelInstance(ILogger logger)
    {
        return new TaskHelper(dialogsService, connectivityService, translatorService) //// TODO use DI?
            .CheckInternetBeforeStarting(true)
            .WithAnalitics(appCenterService)
            .WithLogging(logger);
    }

    public ITaskHelper CreateInternetAccessViewModelInstance(ILogger logger, IBusyViewModel busyViewModel)
    {
        return CreateInternetAccessViewModelInstance(logger)
            .WithAnalitics(appCenterService)
            .WhenStarting(new Action(() => { busyViewModel.IsBusy = true; }))
            .WhenFinished(new Action(() => { busyViewModel.IsBusy = false; }));
    }

    public ITaskHelper CreateSimpleInstance(ILogger logger)
    {
        return new TaskHelper(dialogsService, connectivityService, translatorService)
            .WithLogging(logger)
            .WithAnalitics(appCenterService);
    }
}