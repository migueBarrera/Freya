namespace Freya.Desktop.Core.Framework;

public interface INavigationAwareViewModel
{
    Task OnAppearingAsync(object? parameter = null);
    Task OnDisappearingAsync();
}
