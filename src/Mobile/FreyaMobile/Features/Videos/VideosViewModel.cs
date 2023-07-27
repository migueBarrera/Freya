namespace FreyaMobile.Features.Videos;

public class VideosViewModel : CoreViewModel
{
    private readonly IVideosService videosService;
    private readonly ISessionService sessionService;
    private IEnumerable<VideoModel> videos = Enumerable.Empty<VideoModel>();
    private VideoModel? selectedItem;

    public VideosViewModel(IVideosService videosService, ISessionService sessionService)
    {
        this.videosService = videosService;
        this.SelectionChangedCommand = new AsyncCommand(SelectionChangedAsync);
        this.sessionService = sessionService;
    }

    public IAsyncCommand SelectionChangedCommand { get; set; }

    public VideoModel? SelectedItem { get => selectedItem; set => SetAndRaisePropertyChanged(ref selectedItem, value); }

    public IEnumerable<VideoModel> Videos
    {
        get => videos;
        set => SetAndRaisePropertyChanged(ref videos, value);
    }

    public override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        IsBusy = true;
        Videos = await videosService.GetVideosAsync();
        IsBusy = false;
    }

    private async Task SelectionChangedAsync()
    {
        if (selectedItem == null)
        {
            return;
        }

        if (!selectedItem.Available)
        {
            SelectedItem = null;
            return;
        }

        sessionService.Save(SESSION.VIDEOS_CAPSULE, selectedItem);
        await AppShell.Current.GoToAsync($"{nameof(DetailVideoPage)}");

        SelectedItem = null;
    }
}
