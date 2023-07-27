namespace FreyaMobile.Features.Sounds;

public class DetailSoundViewModel : CoreViewModel
{
    private readonly ISessionService sesionService;
    private SoundModel? soundCapsule;
    private readonly IShareService shareService;

    public DetailSoundViewModel(
        ISessionService sesionService,
        IShareService shareService)
    {
        this.sesionService = sesionService;
        PlayCommand = new AsyncCommand(PlayCommandExecute);
        ShareItemCommand = new AsyncCommand(ShareItemCommandExecute);
        this.shareService = shareService;
    }

    public IAsyncCommand ShareItemCommand { get; set; }


    public SoundModel? Capsule { get => soundCapsule; private set => SetAndRaisePropertyChanged(ref soundCapsule, value); }

    public IAsyncCommand PlayCommand { get; set; }

    public override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();
        Capsule = sesionService.Get<SoundModel>(SESSION.SOUNDS_CAPSULE);
    }

    public override Task OnDisappearingAsync()
    {
        return base.OnDisappearingAsync();
    }

    private Task PlayCommandExecute()
    {
        //await audioService.PlayAsync();
        return Task.CompletedTask;
    }

    private async Task ShareItemCommandExecute()
    {
        var uri = Capsule?.Video;
        if (uri == null)
        {
            return;
        }

        await shareService.ShareSound(uri);
    }
}
