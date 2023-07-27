namespace FreyaMobile.Features.Videos;

public class DetailVideoViewModel : CoreViewModel
{
    private readonly ISessionService sesionService;
    private readonly IShareService shareService;
    private readonly DialogService dialogService;
    private VideoModel? capsule;

    public DetailVideoViewModel(
        ISessionService sesionService,
        IShareService shareService,
        DialogService dialogService)
    {
        this.sesionService = sesionService;
        this.shareService = shareService;

        PlayCommand = new AsyncCommand(PlayCommandExecute);
        ShareItemCommand = new AsyncCommand(ShareItemCommandExecute);
        DownloadItemCommand = new AsyncCommand(DownloadItemCommandExecute);
        this.dialogService = dialogService;
    }

    public IAsyncCommand ShareItemCommand { get; set; }

    public IAsyncCommand DownloadItemCommand { get; set; }

    public VideoModel? Capsule { get => capsule; private set => SetAndRaisePropertyChanged(ref capsule, value); }

    public IAsyncCommand PlayCommand { get; set; }

    public override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();
        Capsule = sesionService.Get<VideoModel>(SESSION.VIDEOS_CAPSULE);
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
        var uri = capsule!.Video;
        if (uri == null)
        {
            return;
        }

        await shareService.ShareVideo(uri);
    }

    private async Task DownloadItemCommandExecute()
    {
        var uri = capsule!.Video;
        if (uri == null)
        {
            return;
        }

        var result = await shareService.DowloadOnGallery(uri);
        if (result)
        {
            await dialogService.DisplayAlert("Descargado", "Mire en su galeria");
        }
    }
}
