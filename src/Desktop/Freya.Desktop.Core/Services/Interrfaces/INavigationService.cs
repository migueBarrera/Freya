namespace Freya.Desktop.Core.Services.Interrfaces;

public interface INavigationService
{
    Task Init(NavigationService navigationService);
    Task NavigateTo(Type page, bool clearStack = false);
    bool CanGoBack();
    Task BackAsync();

    Task ClearStackAsync();
}
