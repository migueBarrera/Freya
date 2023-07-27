using AppCenterServices;
using Freya.Desktop.Core.Features.Clients;
using Freya.Desktop.Core.Features.Clients.Models;
using Freya.Desktop.Core.Features.Clients.Services;

namespace Freya.Features.Clients;

public class ClientDetailViewModel : CoreViewModel, IMultimediaItemRemoveToParentModel
{
    private ISessionService sessionService;
    private IClientDetailService clientDetailService;
    private IMultimediaFilePickerService multimediaFilePickerService;
    private IServiceProvider serviceProvider;
    private ICurrentClinicService currentClinicService;
    private IDialogService dialogService;
    private INavigationService navigationService;
    private ClientDetailModel? client;
    private ILogger<ClientDetailViewModel> logger;
    private ObservableCollection<ImageMultimediaViewModel> ultrasounds;
    private ObservableCollection<VideoMultimediaViewModel> videos;
    private ObservableCollection<SoundMultimediaViewModel> sounds;
    private ISessionManagerService sessionManagerService;
    private AppCenterAnalitics appCenterAnalitics;
    private readonly ITranslatorService translatorService;
    private Guid clinicId;
    private bool clinicHasDiferentSubscriptionPlan;

    public ClientDetailViewModel(
        ISessionService sessionService,
        ILogger<ClientDetailViewModel> logger,
        IClientDetailService clientDetailService,
        IServiceProvider serviceProvider,
        IDialogService dialogService,
        INavigationService navigationService,
        ISessionManagerService sessionManagerService,
        ICurrentClinicService currentClinicService,
        ITranslatorService translatorService,
        AppCenterAnalitics appCenterAnalitics,
        IMultimediaFilePickerService multimediaFilePickerService)
    {
        Title = translatorService.Translate("page_title_client_detail");
        ShowBackButton = true;
        this.sessionService = sessionService;
        this.logger = logger;
        this.clientDetailService = clientDetailService;
        this.serviceProvider = serviceProvider;
        this.dialogService = dialogService;
        this.navigationService = navigationService;
        this.sessionManagerService = sessionManagerService;
        this.currentClinicService = currentClinicService;
        this.translatorService = translatorService;
        this.multimediaFilePickerService = multimediaFilePickerService;
        this.appCenterAnalitics = appCenterAnalitics;

        ultrasounds = new ObservableCollection<ImageMultimediaViewModel>();
        videos = new ObservableCollection<VideoMultimediaViewModel>();
        sounds = new ObservableCollection<SoundMultimediaViewModel>();

        UploadFileCommand = new AsyncCommand(UploadFileCommandExecute);
        UploadVideoCommand = new AsyncCommand(UploadVideoCommandExecute);
        UploadSoundCommand = new AsyncCommand(UploadSoundsCommandExecute);
        DeleteClientCommand = new AsyncCommand(DeleteClientCommandExecute);
        EditClientCommand = new AsyncCommand(EditClientCommandExecute);
        UpdateSubscriptionPlanCommand = new AsyncCommand(UpdateSubscriptionPlanCommandExecute);

        appCenterAnalitics.PageView(nameof(ClientDetailViewModel));
    }

    public void RemoveItem(Guid id)
    {
        var ultrasoundForDelete = ultrasounds.FirstOrDefault(x => x.Data.Id == id);
        if (ultrasoundForDelete != null)
        {
            appCenterAnalitics.Clicked("Removed item - Image");
            Client!.Size -= ultrasoundForDelete.Data.Size;
            Ultrasounds.Remove(ultrasoundForDelete);
            Ultrasounds = Ultrasounds;
            return;
        }

        var videosForDelete = videos.FirstOrDefault(x => x.Data.Id == id);
        if (videosForDelete != null)
        {
            appCenterAnalitics.Clicked("Removed item - Video");
            Client!.Size -= videosForDelete.Data.Size;
            Videos.Remove(videosForDelete);
            Videos = Videos;
            return;
        }

        var soundsForDelete = sounds.FirstOrDefault(x => x.Data.Id == id);
        if (soundsForDelete != null)
        {
            appCenterAnalitics.Clicked("Removed item - Sound");
            Client!.Size -= soundsForDelete.Data.Size;
            Sounds.Remove(soundsForDelete);
            Sounds = Sounds;
            return;
        }
    }

    private async Task UploadFileCommandExecute()
    {
        appCenterAnalitics.Clicked("Upload item - Image");
        await OpenFilePickerAndUploadMultimedia(FileFilters.IMAGE_FILTER, MultimediaType.ULTRASOUND);
    }

    private async Task UploadVideoCommandExecute()
    {
        appCenterAnalitics.Clicked("Upload item - Video");
        await OpenFilePickerAndUploadMultimedia(FileFilters.VIDEO_FILTER, MultimediaType.VIDEO);
    }

    private async Task UploadSoundsCommandExecute()
    {
        appCenterAnalitics.Clicked("Upload item - Sound");
        await OpenFilePickerAndUploadMultimedia(FileFilters.SOUND_FILTER, MultimediaType.SOUND);
    }

    private async Task OpenFilePickerAndUploadMultimedia(string filter, MultimediaType multimediaType)
    {
        await multimediaFilePickerService.OpenFilePickerAndUploadMultimedia(
            filter,
            multimediaType,
            client!.Id,
            clinicId,
            string.Empty,
            $"{client.Name} {client.LastName}",
            client.SubscriptionPlanSize,
            client.Size);
    }

    public IAsyncCommand UploadFileCommand { get; set; }

    public IAsyncCommand UploadVideoCommand { get; set; }

    public IAsyncCommand UploadSoundCommand { get; set; }

    public IAsyncCommand DeleteClientCommand { get; set; }

    public IAsyncCommand EditClientCommand { get; set; }

    public IAsyncCommand UpdateSubscriptionPlanCommand { get; set; }

    public ClientDetailModel? Client { get => client; set => SetAndRaisePropertyChanged(ref client, value); }

    public ObservableCollection<ImageMultimediaViewModel> Ultrasounds { get => ultrasounds; set => SetAndRaisePropertyChanged(ref ultrasounds, value); }

    public ObservableCollection<VideoMultimediaViewModel> Videos { get => videos; set => SetAndRaisePropertyChanged(ref videos, value); }

    public ObservableCollection<SoundMultimediaViewModel> Sounds { get => sounds; set => SetAndRaisePropertyChanged(ref sounds, value); }

    public bool ClinicHasDiferentSubscriptionPlan { get => clinicHasDiferentSubscriptionPlan; set => SetAndRaisePropertyChanged(ref clinicHasDiferentSubscriptionPlan, value); }

    public override async Task OnAppearingAsync(object? parameter = null)
    {
        await base.OnAppearingAsync();
        clinicId = sessionService.Get<Guid>(SESSION.KEY_CLINIC_ID_SELECTED);
        var clientResponse = sessionService.Get<ClientResponse>(SESSION.KEY_CLIENT_SELECTED);
        Client = ClientDetailModel.To(clientResponse);
        IsBusy = true;
        await LoadClientDetails();
        IsBusy = false;

        sessionManagerService.OnNewFileUploadedSuccess += OnFileUploadedSuccess;
    }

    public override Task OnDisappearingAsync()
    {
        sessionManagerService.OnNewFileUploadedSuccess -= OnFileUploadedSuccess;
        return base.OnDisappearingAsync();
    }

    private void OnFileUploadedSuccess(object? sender, SessionViewModel session)
    {
        if (session.ClinicId == clinicId
            && session.ClientId == client!.Id)
        {
            BaseMultimedia baseMultimedia = new BaseMultimedia()
            {
                ClientId = session.ClientId,
                ClinicId = session.ClinicId,
                Uri = session.Uri ?? new Uri("about:blank"),
                Size = session.Size,
                Id = session.BaseMultimediaId ?? Guid.Empty,
            };

            switch (session.MultimediaType)
            {
                case MultimediaType.ULTRASOUND:

                    Ultrasounds.Insert(
                        0,
                        serviceProvider.ConvertToImageMultimediaViewModel(this, new ImageMultimedia(baseMultimedia))!);
                    break;
                case MultimediaType.VIDEO:
                    var vm = serviceProvider.ConvertToVideoMultimediaViewModel(this, new VideoMultimedia(baseMultimedia, session.ThumnailUri ?? new Uri("about:blank")));
                    Videos.Insert(
                        0,
                        vm!);
                    break;
                case MultimediaType.SOUND:
                    Sounds.Insert(
                        0,
                        serviceProvider.ConvertToSoundMultimediaViewModel(this, new SoundMultimedia(baseMultimedia))!);
                    break;
                default:
                    break;
            }
            Client!.Size += session.Size;
        }
    }

    private async Task LoadClientDetails()
    {
        var clientResponse = await clientDetailService.GetClientDetails(Client!.Id, clinicId);
        Client = ClientDetailModel.To(clientResponse);
        Ultrasounds = ConvertToViewModel(clientResponse.Ultrasounds);
        Videos = ConvertToViewModel(clientResponse.Videos);
        Sounds = ConvertToViewModel(clientResponse.Sounds);
        ChecksubcriptionForButtonUpdateState();
    }

    
    private void ChecksubcriptionForButtonUpdateState()
    {
        SubscriptionPaymentResponse? subcritionForCurrentClinic = currentClinicService.CurrentClinic?.CurrentSubscription;
        if (subcritionForCurrentClinic != null && subcritionForCurrentClinic.State == SubscriptionStates.ACTIVE)
        {
            var clinicHasAMayorSubscription = subcritionForCurrentClinic.Size > Client!.SubscriptionPlanSize;
            ClinicHasDiferentSubscriptionPlan = clinicHasAMayorSubscription;
        }
    }

    private ObservableCollection<SoundMultimediaViewModel> ConvertToViewModel(IEnumerable<SoundMultimedia> items)
    {
        var viewmodels = new List<SoundMultimediaViewModel>();

        foreach (var multimediaData in items)
        {
            var vm = serviceProvider.ConvertToSoundMultimediaViewModel(this, multimediaData);
            if (vm != null)
            {
                viewmodels.Add(vm);
            }
        }

        return new ObservableCollection<SoundMultimediaViewModel>(viewmodels);
    }

    private ObservableCollection<ImageMultimediaViewModel> ConvertToViewModel(IEnumerable<ImageMultimedia> items)
    {
        var viewmodels = new List<ImageMultimediaViewModel>();

        foreach (var multimediaData in items)
        {
            var vm = serviceProvider.ConvertToImageMultimediaViewModel(this, multimediaData);
            if (vm != null)
            {
                viewmodels.Add(vm);
            }
        }

        return new ObservableCollection<ImageMultimediaViewModel>(viewmodels);
    }

    private ObservableCollection<VideoMultimediaViewModel> ConvertToViewModel(IEnumerable<VideoMultimedia> items)
    {
        var viewmodels = new List<VideoMultimediaViewModel>();

        foreach (var multimediaData in items)
        {
            var vm = serviceProvider.ConvertToVideoMultimediaViewModel(this, multimediaData);
            if (vm != null)
            {
                viewmodels.Add(vm);
            }
        }

        return new ObservableCollection<VideoMultimediaViewModel>(viewmodels);
    }

    private async Task DeleteClientCommandExecute()
    {
        appCenterAnalitics.Clicked("Delete client");
        await dialogService.ShowMessageYesNo(
                    translatorService.Translate("dialog_client_detail_remove_client_title"),
                    translatorService.Translate("dialog_client_detail_remove_client_body"),
                    new AsyncCommand(async () =>
                    {
                        IsBusy = true;
                        var clinicId = currentClinicService.CurrentClinic?.Id ?? Guid.Empty;
                        var deleted = await clientDetailService.DeleteClient(Client!.Id, clinicId);
                        IsBusy = false;
                        if (deleted)
                        {
                            await navigationService.BackAsync();
                        }
                    }));
    }

    private async Task EditClientCommandExecute()
    {
        appCenterAnalitics.Clicked("Edit client");
        sessionService.Save(SESSION.KEY_CLIENT_DETAIL_SELECTED, ClientDetailModel.To(Client!));
        await navigationService.NavigateTo(typeof(EditClientPage));
    }
    
    private async Task UpdateSubscriptionPlanCommandExecute()
    {
        await dialogService.ShowMessageYesNo(
                    translatorService.Translate("dialog_client_detail_update_plan_title"),
                    translatorService.Translate("dialog_client_detail_update_plan_body"),
                    new AsyncCommand(async () =>
                    {
                        appCenterAnalitics.Clicked("Update subscription client plan");
                        IsBusy = true;
                        var updated = await clientDetailService.UpdateClientPlan(client!.Id, clinicId);
                        if (updated) 
                        {
                            await LoadClientDetails();
                        }
                        IsBusy = false;
                    }));
    }
}
