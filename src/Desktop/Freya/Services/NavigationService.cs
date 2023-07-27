using System.Windows.Controls;
using System.Windows.Navigation;

namespace Freya.Services;

internal class NavigationService : INavigationService
{
    private System.Windows.Navigation.NavigationService? windowsNavigationService;
    private readonly IServiceProvider serviceProvider;

    public NavigationService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public EventHandler? NavigationStackChanged { get; set; }

    public Task BackAsync()
    {
        if (windowsNavigationService?.CanGoBack ?? false)
        {
            windowsNavigationService.GoBack();
        }

        NavigationStackChanged?.Invoke(this, EventArgs.Empty);

        return Task.CompletedTask;
    }

    public bool CanGoBack()
    {
        return windowsNavigationService?.CanGoBack ?? false;
    }

    public Task ClearStackAsync()
    {
        while (windowsNavigationService?.CanGoBack ?? false)
        {
            windowsNavigationService.RemoveBackEntry();
        }

        return Task.CompletedTask;
    }

    public Task Init(System.Windows.Navigation.NavigationService navigationService)
    {
        windowsNavigationService = navigationService;
        return NavigateTo(typeof(SignInPage), true);
    }

    public Task NavigateTo(Type pageType, bool clearStack)
    {
        var page = serviceProvider.GetService(pageType) as Page;
        var viewmodel = (page?.DataContext as CoreViewModel);
        if (viewmodel != null)
        {
            viewmodel.Parent = serviceProvider.GetService<MainViewModel>();
        }

        if (clearStack)
        {
            ClearHistory();
        }

        windowsNavigationService?.Navigate(page);

        NavigationStackChanged?.Invoke(this, EventArgs.Empty);

        return Task.CompletedTask;
    }

    public void ClearHistory()
    {
        var canGoBack = windowsNavigationService?.CanGoBack ?? false;
        var canGoForward = windowsNavigationService?.CanGoForward ?? false;
        if (!canGoBack && !canGoForward)
        {
            return;
        }

        var entry = this.windowsNavigationService?.RemoveBackEntry();
        while (entry != null)
        {
            entry = this.windowsNavigationService?.RemoveBackEntry();
        }

        this.windowsNavigationService?.Navigate(new PageFunction<string>() { RemoveFromJournal = true });
    }
}
