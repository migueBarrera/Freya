namespace FreyaMobile.Core.Services;

public class DialogService
{
    public Task DisplayAlert(string title, string message, string close = "Cerrar")
    {
        if (Application.Current?.MainPage == null)
        {
            return Task.CompletedTask;
        }

        return Application.Current.MainPage.DisplayAlert(
                title,
                message,
                close);
    }

    public Task PopAsync()
    {
        if (Application.Current?.MainPage == null)
        {
            return Task.CompletedTask;
        }

        return Application.Current.MainPage.Navigation.PopAsync();
    }
}
