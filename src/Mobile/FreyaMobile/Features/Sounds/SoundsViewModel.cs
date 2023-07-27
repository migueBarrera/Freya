namespace FreyaMobile.Features.Sounds;

public class SoundsViewModel : CoreViewModel
{
    private readonly SoundsService soundService;
    private readonly ISessionService sessionService;
    private IEnumerable<SoundModel> sounds = Enumerable.Empty<SoundModel>();
    private SoundModel? selectedItem;

    public SoundsViewModel(
        SoundsService soundService, 
        ISessionService sessionService)
    {
        this.soundService = soundService;
        this.SelectionChangedCommand = new AsyncCommand(SelectionChangedAsync);
        this.sessionService = sessionService;
    }

    public IAsyncCommand SelectionChangedCommand { get; set; }

    public SoundModel? SelectedItem { get => selectedItem; set => SetAndRaisePropertyChanged(ref selectedItem, value); }

    public IEnumerable<SoundModel> Sounds
    {
        get => sounds;
        set => SetAndRaisePropertyChanged(ref sounds, value);
    }

    public override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        IsBusy = true;
        Sounds = await soundService.GetSounds();
        IsBusy = false;
    }

    private async Task SelectionChangedAsync()
    {
        if (selectedItem == null)
        {
            return;
        }

        sessionService.Save(SESSION.SOUNDS_CAPSULE, selectedItem);
        await AppShell.Current.GoToAsync($"{nameof(DetailSoundPage)}");
        SelectedItem = null;
    }
}
