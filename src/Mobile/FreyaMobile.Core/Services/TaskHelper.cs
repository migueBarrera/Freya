using static OperationResult.Helpers;

namespace FreyaMobile.Core.Services;

public class TaskHelper : ITaskHelper
{
    private readonly DialogService dialogsService;
    private readonly IConnectivityService connectivityService;
    private readonly IAuthenticationService authenticationService;

    private bool checkInternetAccess;
    private Action? whenStarting;
    private Action? whenFinished;
    private Func<Exception, Task<bool>>? errorHandler;
    private ILoggingService? logger;

    public TaskHelper(
        DialogService dialogsService,
        IConnectivityService connectivityService,
        IAuthenticationService authenticationService)
    {
        this.dialogsService = dialogsService;
        this.connectivityService = connectivityService;
        this.authenticationService = authenticationService;
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

    public ITaskHelper WithLogging(ILoggingService? logger)
    {
        this.logger = logger;

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

            logger?.Error(exception);

            if (!isAlreadyHandled)
            {
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
        var counter = 0;
        while (!connectivityService.IsThereInternet && counter<=2)
        {
            counter++;
            // TODO poder salir del bucle peticion de usuario en lugar de bucle
            await dialogsService.DisplayAlert(
                "This application requires an active Internet connection to work.",
                "There is no internet access",
                "Close");
        }

        return !connectivityService.IsThereInternet;
    }

    private async Task<bool> HandleCommonExceptionsAsync(Exception exception)
    {
        if (exception is HttpRequestException || exception is ApiException)
        {
            if (exception is ApiException apiException && apiException.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await dialogsService.DisplayAlert(
                   "Ha caducado su sesion",
                   "Sesión caducada",
                "Close");
                await authenticationService.SignOut();
            }
            else
            {
                await dialogsService.DisplayAlert(
               "An error was detected while communicating with server. Please, try again later.",
               "Data error",
                "Close");
            }

            return true;
        }

        return false;
    }
}