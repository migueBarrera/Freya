namespace Freya.Services.UploadManager;

public class SessionViewModel : SessionModel
{
    private WasabiStorageService azureUploaderService;
    private SessionHelper sessionHelper;
    private readonly ILogger<SessionViewModel> logger;
    public static string FileUploadSearchPattern = "*";
    private IMultimediaAPIService multimediaAPIService;
    private ITaskHelperFactory taskHelperFactory;
    private readonly AppCenterAnalitics appCenterAnalitics;
    private readonly AppCenterService appCenterService;

    public SessionViewModel(
        WasabiStorageService azureUploaderService,
        IRefitService refitService,
        ILogger<SessionViewModel> logger,
        ITaskHelperFactory taskHelperFactory,
        SessionHelper sessionHelper,
        AppCenterAnalitics appCenterAnalitics,
        AppCenterService appCenterService) : base()
    {
        this.azureUploaderService = azureUploaderService;
        this.logger = logger;
        this.sessionHelper = sessionHelper;
        this.taskHelperFactory = taskHelperFactory;
        this.appCenterAnalitics = appCenterAnalitics;
        this.multimediaAPIService = refitService.InitRefitInstance<IMultimediaAPIService>(isAutenticated: true);

        RetryCommand = new AsyncCommand(RetryCommandExecute);
        DeleteCommand = new AsyncCommand(DeleteCommandExecute);
        this.appCenterService = appCenterService;
    }

    public IAsyncCommand RetryCommand { get; set; }

    public IAsyncCommand DeleteCommand { get; set; }

    public EventHandler<Guid>? OnNewFileUploading { get; set; }

    public EventHandler<Guid>? OnNewFileUploadedSuccess { get; set; }

    public EventHandler<Guid>? OnNewFileUploadedFailed { get; set; }

    public EventHandler<Guid>? OnFileUploadRetry { get; set; }

    public EventHandler<Guid>? OnFileUploadCancel { get; set; }

    private Task DeleteCommandExecute()
    {
        appCenterAnalitics.Clicked("Cancel upload multimedia");
        OnFileUploadCancel?.Invoke(this, SesionId);

        return Task.CompletedTask;
    }

    private Task RetryCommandExecute()
    {
        appCenterAnalitics.Clicked("Retry upload multimedia");
        Task.Run(Retry);
        return Task.CompletedTask;
    }

    public async Task<bool> StartUpload()
    {
        appCenterAnalitics.UploadMultimedia(ClinicId, MultimediaType.ToString());
        var response = await taskHelperFactory.
                                CreateSimpleInstance(logger).
                                WithErrorHandling(OnCreateMultimediaError).
                                TryExecuteAsync(() => InternalStartUpload());

        return response.IsSuccess;
    }

    public async Task<bool> Retry()
    {
        var response = await taskHelperFactory.
                               CreateSimpleInstance(logger).
                               WithErrorHandling(OnCreateMultimediaError).
                               TryExecuteAsync(() => InternalRetry());

        return response.IsSuccess;
    }

    private async Task InternalRetry()
    {
        OnFileUploadRetry?.Invoke(this, SesionId);
        await Upload();
    }

    private async Task InternalStartUpload()
    {
        OnNewFileUploading?.Invoke(this, SesionId);

        var file = new FileInfo(Path);
        Size = file.Length;

        await Upload();
    }

    private async Task Upload()
    {
        logger.LogInformation($"Trying upload a file {Path}");

        var resultResponse = await azureUploaderService.Upload(ClientId, Path);
        logger.LogInformation($"Azure response for file upload {Path} , {resultResponse}");

        appCenterAnalitics.UploadMultimediaState(resultResponse.Success ? "success" : "failed", ClinicId);
        if (resultResponse.Success)
        {
            Uri? thumnailUri = null;
            if (MultimediaType == MultimediaType.VIDEO)
            {
                thumnailUri = await sessionHelper.TryGetAndUploadThumnail(ClientId, Path);
                ThumnailUri = thumnailUri;
            }

            await NotifyApiMultimedia(this, resultResponse.Uri!, Size, thumnailUri);
        }
        else
        {
            OnNewFileUploadedFailed?.Invoke(this, SesionId);
            logger.LogInformation($"Send event {nameof(OnNewFileUploadedFailed)} {Path}");
        }
    }

    private async Task NotifyApiMultimedia(SessionViewModel session, Uri uri, long fileLength, Uri? thumnailUri)
    {
        logger.LogInformation($"Trying notify to server about file upload size: {fileLength}, URI: {uri} , clinic: {session.ClinicName}, client: {session.ClientName}");
        var result = await multimediaAPIService.CreateMultimedia(new CreateMultimediaRequest(
            uri,
            session.ClinicId,
            session.ClientId,
            session.MultimediaType,
            fileLength,
        thumnailUri));

        session.BaseMultimediaId = result;
        session.Uri = uri;

        OnNewFileUploadedSuccess?.Invoke(this, session.SesionId);
        logger.LogInformation($"Send event {nameof(OnNewFileUploadedFailed)} {session.Path}");
    }

    private Task<bool> OnCreateMultimediaError(Exception arg)
    {
        appCenterService.TrackError(arg);
        logger.LogInformation($"Error uploading a file {arg}");
        OnNewFileUploadedFailed?.Invoke(this, SesionId);
        if (arg is ApiException apiException)
        {
            return Task.FromResult(true);
        }
        else
        {
            return Task.FromResult(false);
        }
    }
}
