namespace Freya.Features.Main;

public class HeaderControlViewModel : CoreViewModel
{
    private readonly ICurrentClinicService currentClinicService;
    private readonly ISessionManagerService sessionManagerService;
    private ClinicResponse? currentClinic;
    private int countUploads;

    public HeaderControlViewModel(
        ICurrentClinicService currentClinicService,
        ISessionManagerService sessionManagerService)
    {
        this.currentClinicService = currentClinicService;
        this.sessionManagerService = sessionManagerService;

        Clinics = new ObservableCollection<ClinicResponse>();

        sessionManagerService.OnNewFileUploading += OnNewFileUploading;
        sessionManagerService.OnNewFileUploadedSuccess += OnFileUploadedSuccess;
        sessionManagerService.OnNewFileUploadedFailed += OnFileUploadedFailed;
        sessionManagerService.OnNewFileUploadedCanceled += OnNewFileUploadedCanceled;
    }

    private void OnNewFileUploadedCanceled(object? sender, SessionViewModel e)
    {
        CountUploads -= 1;
    }

    private void OnFileUploadedFailed(object? sender, SessionViewModel e)
    {
        //CountUploads -= 1;
    }

    private void OnFileUploadedSuccess(object? sender, SessionViewModel e)
    {
        CountUploads -= 1;
    }

    private void OnNewFileUploading(object? sender, SessionViewModel e)
    {
        CountUploads += 1;
    }

    public ObservableCollection<ClinicResponse> Clinics { get; set; }

    public ObservableCollection<SessionViewModel> UploadsItems { get => sessionManagerService.GetCurrentsSessions(); }

    public ClinicResponse? CurrentClinic
    {
        get => currentClinic;
        set
        {
            SetAndRaisePropertyChanged(ref currentClinic, value);
            currentClinicService.CurrentClinic = value;
        }
    }

    public int CountUploads { get => countUploads; set => SetAndRaisePropertyChanged(ref countUploads, value); }

    public override async Task OnAppearingAsync(object? parameter = null)
    {
        await base.OnAppearingAsync();
        Clinics.Clear();
        foreach (var clinic in currentClinicService.Clinics)
        {
            Clinics.Add(clinic);
        }
        if (Clinics.Any())
        {
            CurrentClinic = Clinics.First();
        }
    }

    internal void SetClinic(Guid clinicSessionId, string name)
    {
        var clinicForSelect = Clinics.FirstOrDefault(x => x.Id == clinicSessionId);

        if (clinicForSelect == null)
        {
            currentClinicService.AddClinic(new ClinicResponse { Id = clinicSessionId , Name = name,});
            Clinics.Add(new ClinicResponse { Id = clinicSessionId, Name = name, });
            clinicForSelect = Clinics.FirstOrDefault(x => x.Id == clinicSessionId);
        }

        if (clinicForSelect != null)
        {
            CurrentClinic = clinicForSelect;
        }
    }
    
    internal void RemoveClinic(Guid clinicSessionId)
    {
        var clinicForRemove = Clinics.FirstOrDefault(x => x.Id == clinicSessionId);

        if (clinicForRemove == null)
        {
            return;
        }

        currentClinicService.RemoveClinic(clinicSessionId);
        Clinics.Remove(clinicForRemove);
        CurrentClinic = Clinics.FirstOrDefault();
        currentClinicService.CurrentClinic = CurrentClinic;
    }
}
