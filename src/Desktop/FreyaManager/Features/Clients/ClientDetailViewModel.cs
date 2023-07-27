namespace FreyaManager.Features.Clients;

public class ClientDetailViewModel : CoreViewModel, IMultimediaItemRemoveToParentModel
{
    private readonly ISessionService sessionService;
    private readonly IClientDetailService clientDetailService;
    private readonly IServiceProvider serviceProvider;
    private readonly IDialogService dialogService;
    private readonly INavigationService navigationService;
    private readonly ILogger<ClientDetailViewModel> logger;
    private ClientDetailModel? client;
    private ObservableCollection<ImageMultimediaViewModel> ultrasounds;
    private ObservableCollection<VideoMultimediaViewModel> videos;
    private ObservableCollection<SoundMultimediaViewModel> sounds;
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
        ITranslatorService translatorService,
        AppCenterAnalitics appCenterAnalitics)
    {
        Title = translatorService.Translate("page_title_client_detail");
        ShowBackButton = true;
        this.sessionService = sessionService;
        this.logger = logger;
        this.clientDetailService = clientDetailService;
        this.serviceProvider = serviceProvider;
        this.dialogService = dialogService;
        this.navigationService = navigationService;
        this.translatorService = translatorService;
        this.appCenterAnalitics = appCenterAnalitics;

        ultrasounds = new ObservableCollection<ImageMultimediaViewModel>();
        videos = new ObservableCollection<VideoMultimediaViewModel>();
        sounds = new ObservableCollection<SoundMultimediaViewModel>();

        DeleteClientCommand = new AsyncCommand(DeleteClientCommandExecute);
        EditClientCommand = new AsyncCommand(EditClientCommandExecute);

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

    public IAsyncCommand DeleteClientCommand { get; set; }

    public IAsyncCommand EditClientCommand { get; set; }

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
    }

    public override Task OnDisappearingAsync()
    {
        return base.OnDisappearingAsync();
    }

  
    private async Task LoadClientDetails()
    {
        var clientResponse = await clientDetailService.GetClientDetails(Client!.Id, clinicId);
        Client = ClientDetailModel.To(clientResponse);
        Ultrasounds = ConvertToViewModel(clientResponse.Ultrasounds);
        Videos = ConvertToViewModel(clientResponse.Videos);
        Sounds = ConvertToViewModel(clientResponse.Sounds);
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
}
