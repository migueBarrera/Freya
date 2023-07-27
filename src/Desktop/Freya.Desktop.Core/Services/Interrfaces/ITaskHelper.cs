namespace Freya.Desktop.Core.Services.Interrfaces;

public interface ITaskHelper
{
    ITaskHelper CheckInternetBeforeStarting(bool check);

    ITaskHelper WithLogging(ILogger logger);

    ITaskHelper WhenStarting(Action action);

    ITaskHelper WhenFinished(Action action);

    ITaskHelper WithAnalitics(AppCenterService analitics);

    ITaskHelper WithErrorHandling(Func<Exception, Task<bool>> handler);

    Task<Status> TryExecuteAsync(Func<Task> task);

    Task<Result<T>> TryExecuteAsync<T>(Func<Task<T>> task);
}
