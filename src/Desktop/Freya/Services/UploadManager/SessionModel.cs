namespace Freya.Services.UploadManager;

public class SessionModel : ObservableObject
{
    private string path = string.Empty;
    private bool isInProgress;
    private bool isFailed;

    public SessionModel()
    {
        SesionId = Guid.NewGuid();
    }

    public Guid SesionId { get; set; } 

    public Guid? BaseMultimediaId { get; set; }

    public string Path { get => path; set => SetAndRaisePropertyChanged(ref path, value); }

    public Guid ClinicId { get; set; } = Guid.Empty;

    public Guid ClientId { get; set; } = Guid.Empty;

    public string ClientName { get; set; } = string.Empty;

    public string ClinicName { get; set; } = string.Empty;

    public Uri? Uri { get; set; }

    public Uri? ThumnailUri { get; set; }

    public long Size { get; set; }

    public MultimediaType MultimediaType { get; set; } = MultimediaType.ULTRASOUND;

    public bool IsInProgress { get => isInProgress; set => SetAndRaisePropertyChanged(ref isInProgress, value); }

    public bool IsFailed { get => isFailed; set => SetAndRaisePropertyChanged(ref isFailed, value); }
}
