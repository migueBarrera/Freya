namespace FreyaMobile.Core.Framework;

public abstract class CorePopupViewModel : CoreViewModel
{
    public CorePopupViewModel()
    {
        ClosePopupCommand = new AsyncCommand<Popup>(ClosePopupAsync);
    }

    public Popup? Popup { get; set; }

    public AsyncCommand<Popup> ClosePopupCommand { get; set; }

    //We need close on MainThread for IOS, if not, throw an exception, EnsureUIThread(). 22/12/22
    protected virtual Task ClosePopupAsync(Popup popup)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            popup?.Close();
        });

        return Task.CompletedTask;
    }


    protected virtual Task ClosePopupAsync()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Popup?.Close();
        });
        
        return Task.CompletedTask;
    }

}
