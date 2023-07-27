namespace Freya.Desktop.Core.Services.Interrfaces;

public interface ITaskHelperFactory
{
    ITaskHelper CreateSimpleInstance(ILogger logger);

    ITaskHelper CreateInternetAccessViewModelInstance(ILogger logger);

    ITaskHelper CreateInternetAccessViewModelInstance(ILogger logger, IBusyViewModel busyViewModel);
}
