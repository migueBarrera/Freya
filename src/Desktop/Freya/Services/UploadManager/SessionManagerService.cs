namespace Freya.Services.UploadManager;

public class SessionManagerService : ISessionManagerService
{
    private ObservableCollection<SessionViewModel> sessions = new ObservableCollection<SessionViewModel>();
    private readonly ILogger<SessionManagerService> logger;
    
    private readonly Application app;

    public SessionManagerService(
        ILogger<SessionManagerService> logger,
        Application app,
        AppCenterAnalitics appCenterAnalitics)
    {
        this.logger = logger;
        this.app = app;
    }

    public EventHandler<SessionViewModel>? OnNewFileUploading { get; set; }

    public EventHandler<SessionViewModel>? OnNewFileUploadedSuccess { get; set; }

    public EventHandler<SessionViewModel>? OnNewFileUploadedFailed { get; set; }

    public EventHandler<SessionViewModel>? OnNewFileUploadedCanceled { get; set; }

    public Task<bool> AddNewMultimedia(SessionViewModel session)
    {
        session.OnNewFileUploading += OnFileUploading;
        session.OnNewFileUploadedSuccess += OnFileUploadedSuccess;
        session.OnNewFileUploadedFailed += OnFileUploadedFailed;
        session.OnFileUploadRetry += OnFileUploadRetry;
        session.OnFileUploadCancel += OnFileUploadCancel;
        
        sessions.Add(session);
        
        Task.Run(() => session.StartUpload());
        return Task.FromResult(true);
    }

    private void OnFileUploadCancel(object? sender, Guid sesionId)
    {
        var session = sessions.First(x => x.SesionId == sesionId);

        app.Dispatcher.Invoke(() =>
        {
            sessions.Remove(session);
            OnNewFileUploadedCanceled?.Invoke(this, session);
        });
    }

    private void OnFileUploadRetry(object? sender, Guid sesionId)
    {
        var session = sessions.First(x => x.SesionId == sesionId);

        session.IsInProgress = true;
        session.IsFailed = false;
    }

    private void OnFileUploadedSuccess(object? sender, Guid sesionId)
    {
        var session = sessions.First(x => x.SesionId == sesionId);

        app.Dispatcher.Invoke(() => 
        {
            sessions.Remove(session);
            OnNewFileUploadedSuccess?.Invoke(this, session);
        });
    }

    private void OnFileUploadedFailed(object? sender, Guid sesionId)
    {
        var session = sessions.First(x => x.SesionId == sesionId);

        app.Dispatcher.Invoke(() =>
        {
            session.IsInProgress = false;
            session.IsFailed = true;

            OnNewFileUploadedFailed?.Invoke(this, session);
        });
    }

    private void OnFileUploading(object? sender, Guid sesionId)
    {
        var session = sessions.First(x => x.SesionId == sesionId);

        app.Dispatcher.Invoke(() =>
        {
            session.IsFailed = false;
            session.IsInProgress = true;

            OnNewFileUploading?.Invoke(this, session);
        });
    }

    public ObservableCollection<SessionViewModel> GetCurrentsSessions()
    {
        return sessions;
    }
}
