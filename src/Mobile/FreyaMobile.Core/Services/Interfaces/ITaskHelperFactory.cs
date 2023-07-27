namespace FreyaMobile.Core.Services.Interfaces;

public interface ITaskHelperFactory
{
    ITaskHelper CreateInternetAccessViewModelInstance(ILoggingService? logger);

    ITaskHelper CreateInternetAccessViewModelInstance(ILoggingService logger, IBusyViewModel busyViewModel);
}
