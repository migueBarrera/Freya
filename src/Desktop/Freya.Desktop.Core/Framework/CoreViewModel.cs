namespace Freya.Desktop.Core.Framework;

public class CoreViewModel :
    ObservableObject,
    INavigationAwareViewModel,
    IBusyViewModel
{
    private bool isBusy;
    private string title = string.Empty;
    private bool showBackButton = false;

    public bool IsBusy
    {
        get => isBusy;
        set => SetAndRaisePropertyChanged(ref isBusy, value);
    }

    public CoreViewModel? Parent { get; set; }

    public string Title { get => title; set => SetAndRaisePropertyChanged(ref title!, value); }

    public bool ShowBackButton { get => showBackButton; set => SetAndRaisePropertyChanged(ref showBackButton, value); }
    //TODO mover , no se necesit en manager
    public bool ShowClinicSelector { get; set; } = false;

    public virtual Task OnAppearingAsync(object? parameter = null)
    {
        return Task.CompletedTask;
    }

    public virtual Task OnDisappearingAsync()
    {
        return Task.CompletedTask;
    }
}
