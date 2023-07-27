namespace FreyaMobile.Core.Services;

public class TaskHelperFactory : ITaskHelperFactory
{
    private readonly DialogService dialogsService;
    private readonly IConnectivityService connectivityService;
    private readonly IAuthenticationService authenticationService;

    public TaskHelperFactory(DialogService dialogsService, IConnectivityService connectivityService, IAuthenticationService authenticationService)
    {
        this.dialogsService = dialogsService;
        this.connectivityService = connectivityService;
        this.authenticationService = authenticationService;
    }

    public ITaskHelper CreateInternetAccessViewModelInstance(ILoggingService? logger)
    {
        return new TaskHelper(dialogsService, connectivityService, authenticationService) //// TODO use DI?
            .CheckInternetBeforeStarting(true)
            .WithLogging(logger);
    }

    public ITaskHelper CreateInternetAccessViewModelInstance(ILoggingService logger, IBusyViewModel busyViewModel)
    {
        return CreateInternetAccessViewModelInstance(logger)
            .WhenStarting(() => MainThread.BeginInvokeOnMainThread(() => busyViewModel.IsBusy = true))
            .WhenFinished(() => MainThread.BeginInvokeOnMainThread(() => busyViewModel.IsBusy = false));
    }
}