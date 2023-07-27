namespace Freya.Services.UploadManager;

public interface ISessionManagerService
{
    ObservableCollection<SessionViewModel> GetCurrentsSessions();

    Task<bool> AddNewMultimedia(SessionViewModel session);

    public EventHandler<SessionViewModel>? OnNewFileUploading { get; set; }

    public EventHandler<SessionViewModel>? OnNewFileUploadedSuccess { get; set; }

    public EventHandler<SessionViewModel>? OnNewFileUploadedFailed { get; set; }

    public EventHandler<SessionViewModel>? OnNewFileUploadedCanceled { get; set; }
}
