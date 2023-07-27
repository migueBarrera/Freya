namespace Freya.Features.Faq;

public class FaqItemViewModel : CoreViewModel
{
    private AppCenterAnalitics appCenterAnalitics;
    private bool isOpen;

    public FAQ? Data { get; set; }

    public FaqItemViewModel(AppCenterAnalitics appCenterAnalitics)
    {
        this.appCenterAnalitics = appCenterAnalitics;
        OpenCommand = new AsyncCommand(OpenCommandExecute);
    }

    public string Main { get; set; } = string.Empty;

    public string Desc { get; set; } = string.Empty;

    private Task OpenCommandExecute()
    {
        appCenterAnalitics.Clicked("Faq expanded");
        IsOpen = !IsOpen;
        return Task.CompletedTask;
    }

    public override Task OnAppearingAsync(object? parameter = null)
    {
        Data = parameter is FAQ ? (FAQ)parameter : new FAQ();
        Main = Data.GetTitle();
        Desc = Data.GetDesc();
        return base.OnAppearingAsync(parameter);
    }

    public bool IsOpen { get => isOpen; set => SetAndRaisePropertyChanged(ref isOpen, value); }

    public IAsyncCommand OpenCommand { get; set; }
}

