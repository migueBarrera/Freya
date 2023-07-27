using FreyaMobile.Features.Images.Models;

namespace FreyaMobile.Features.Images;

public class ImagesViewModel : CoreViewModel
{
    private readonly IImagesService imagesService;
    private readonly ISessionService sessionService;

    private ImageModel? selectedImage;
    private IEnumerable<ImageModel> images = Enumerable.Empty<ImageModel>();

    public ImagesViewModel(IImagesService imagesService,  ISessionService sessionService)
    {
        this.imagesService = imagesService;
        this.SelectionChangedCommand = new AsyncCommand(SelectionChangedAsync);
        this.sessionService = sessionService;
    }

    public IAsyncCommand SelectionChangedCommand { get; set; }

    public IEnumerable<ImageModel> Images
    {
        get => images;
        set => SetAndRaisePropertyChanged(ref images, value);
    }

    public ImageModel? SelectedImage { get => selectedImage; set => SetAndRaisePropertyChanged(ref selectedImage, value); }

    public override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        IsBusy = true;
        Images = await imagesService.GetImagesAsync();
        IsBusy = false;
    }

    private async Task SelectionChangedAsync()
    {
        if (selectedImage == null || selectedImage.Image == null)
        {
            return;
        }

        var capsule = new ImagesCapsule()
        {
            Images = this.images,
            Position = this.images.ToList().FindIndex(images => images == selectedImage),
        };

        SelectedImage = null;
        sessionService.Save(SESSION.IMAGES_CAPSULE, capsule);
        await AppShell.Current.GoToAsync($"{nameof(DetailImagePage)}");
        //var page = new CarrouselImagesPage
        //{
        //    Parameter = capsule
        //};

        //await App.Current.MainPage.Navigation.PushAsync(page);

    }
}
