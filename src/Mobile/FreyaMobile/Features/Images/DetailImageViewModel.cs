using FreyaMobile.Features.Images.Models;

namespace FreyaMobile.Features.Images;

public class DetailImageViewModel : CoreViewModel
{
    private readonly ISessionService sesionService;
    private readonly IShareService shareService;
    private readonly DialogService dialogService;
    private ImagesCapsule? imagesCapsule;
    private int currentPosition;

    public DetailImageViewModel(
        ISessionService sesionService,
        IShareService shareService,
        DialogService dialogService)
    {
        this.sesionService = sesionService;
        this.shareService = shareService;

        ShareItemCommand = new AsyncCommand(ShareItemCommandExecute);
        DownloadItemCommand = new AsyncCommand(DownloadItemCommandExecute);
        this.dialogService = dialogService;
    }

    public IAsyncCommand ShareItemCommand { get; set; }

    public IAsyncCommand DownloadItemCommand { get; set; }

    public ImagesCapsule? ImagesCapsule { get => imagesCapsule; private set => SetAndRaisePropertyChanged(ref imagesCapsule, value); }

    public int CurrentPosition 
    { 
        get => currentPosition; 
        set => SetAndRaisePropertyChanged(ref currentPosition, value); 
    }

    public override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();
        ImagesCapsule = sesionService.Get<ImagesCapsule>(SESSION.IMAGES_CAPSULE);
        await Task.Delay(300);
        CurrentPosition = ImagesCapsule.Position;
    }

    private async Task ShareItemCommandExecute()
    {
        var uri = imagesCapsule!.Images.ElementAt(CurrentPosition).Image;
        if (uri == null)
        {
            return;
        }

        await shareService.ShareImage(uri);
    }
    
    private async Task DownloadItemCommandExecute()
    {
        var uri = imagesCapsule!.Images.ElementAt(CurrentPosition).Image;
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
