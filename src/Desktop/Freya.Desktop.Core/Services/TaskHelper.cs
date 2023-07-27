namespace Freya.Desktop.Core.Services;

public class TaskHelper : ITaskHelper
{
    private readonly IDialogService dialogsService;
    private readonly IConnectivityService connectivityService;
    private readonly ITranslatorService translatorService;

    private bool checkInternetAccess;
    private Action? whenStarting;
    private Action? whenFinished;
    private Func<Exception, Task<bool>>? errorHandler;
    private ILogger? logger;
    private AppCenterService? analitics;

    public TaskHelper(
        IDialogService dialogsService,
        IConnectivityService connectivityService,
        ITranslatorService translatorService)
    {
        this.dialogsService = dialogsService;
        this.connectivityService = connectivityService;
        this.translatorService = translatorService;
    }

    public ITaskHelper CheckInternetBeforeStarting(bool check)
    {
        checkInternetAccess = check;

        return this;
    }

    public ITaskHelper WhenStarting(Action action)
    {
        whenStarting = action;

        return this;
    }

    public ITaskHelper WhenFinished(Action action)
    {
        whenFinished = action;

        return this;
    }

    public ITaskHelper WithLogging(ILogger logger)
    {
        this.logger = logger;

        return this;
    }
    
    public ITaskHelper WithAnalitics(AppCenterService analitics)
    {
        this.analitics = analitics;

        return this;
    }

    public ITaskHelper WithErrorHandling(Func<Exception, Task<bool>> handler)
    {
        errorHandler = handler;

        return this;
    }

    public async Task<Status> TryExecuteAsync(Func<Task> task)
    {
        var taskWrapper = new Func<Task<object>>(() => WrapTaskAsync(task));
        var result = await TryExecuteAsync(taskWrapper);

        if (result)
        {
            return Ok();
        }

        return Error();
    }

    public async Task<Result<T>> TryExecuteAsync<T>(Func<Task<T>> task)
    {
        if (checkInternetAccess)
        {
            bool abort = await ExecuteInternetAccessLoopAsync();

            if (abort)
            {
                return Error();
            }
        }

        whenStarting?.Invoke();

        Result<T> result = Error();

        try
        {
            var actualResult = await task();
            result = Ok(actualResult);
        }
        catch (Exception exception)
        {
            var isAlreadyHandled = false;

            if (errorHandler != null)
            {
                isAlreadyHandled = await errorHandler.Invoke(exception);
            }

            if (!isAlreadyHandled)
            {
                analitics?.TrackError(exception);
                logger?.LogError(exception, exception.Message);
                isAlreadyHandled = await HandleCommonExceptionsAsync(exception);
            }

            if (!isAlreadyHandled)
            {
                throw;
            }
        }
        finally
        {
            whenFinished?.Invoke();
        }

        return result;
    }

    private static async Task<object> WrapTaskAsync(Func<Task> innerTask)
    {
        await innerTask();

        return new object();
    }

    private async Task<bool> ExecuteInternetAccessLoopAsync()
    {
        while (!connectivityService.IsThereInternet)
        {
            await dialogsService.ShowMessage(
                translatorService.Translate("dialog_error_no_internet_title"),
                translatorService.Translate("dialog_error_no_internet_body"));
        }

        return !connectivityService.IsThereInternet;
    }

    private async Task<bool> HandleCommonExceptionsAsync(Exception exception)
    {
        if (exception is HttpRequestException || exception is ApiException)
        {
            if (exception is ApiException apiException)
            {
                if (apiException.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    await dialogsService.ShowMessage(
                        translatorService.Translate("dialog_error_response_forbidden_title"),
                        translatorService.Translate("dialog_error_response_forbidden_body"));
                }
                else if(apiException.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await dialogsService.ShowMessage(
                        translatorService.Translate("dialog_error_response_unauthorized_title"),
                        translatorService.Translate("dialog_error_response_unauthorized_body"));
                }
            }
            else
            {
                await dialogsService.ShowMessage(
                     translatorService.Translate("error"),
                    translatorService.Translate("dialog_error_server_error_body"));
            }

            return true;
        }
        else if (exception is TaskCanceledException)
        {
            await dialogsService.ShowMessage(
                    translatorService.Translate("dialog_error_server_not_response_title"),
                    translatorService.Translate("dialog_error_server_not_response_body"));

            return true;
        }

        return false;
    }
}