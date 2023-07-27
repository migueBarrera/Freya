namespace FreyaMobile.Features.Clinics.Services;

public class MenuClinicService
{
    private readonly ITranslatorService translatorService;

    public MenuClinicService(ITranslatorService translatorService)
    {
        this.translatorService = translatorService;
    }

    public IEnumerable<ClinicMenuItem> GenerateMenu()
    {
        return new ClinicMenuItem[]
        {
            new ClinicMenuItem()
            { 
                Title = translatorService.Translate("menu_secction_images"),
                Image = ImageSource.FromFile("image_image.jpg"),
                Type = TypeClinicMenu.IMAGES 
            },
            new ClinicMenuItem()
            { 
                Title = translatorService.Translate("menu_secction_videos"),
                Image = ImageSource.FromFile("video_image.jpg"),
                Type = TypeClinicMenu.VIDEOS 
            },
            new ClinicMenuItem()

            { 
                Title = translatorService.Translate("menu_secction_sounds"),
                Image = ImageSource.FromFile("sound_image.jpg"),
                Type = TypeClinicMenu.LATIDOS 
            },
        };
    }

    public string GetPageFromMenuItemType(TypeClinicMenu type)
    {
        string page = type switch
        {
            TypeClinicMenu.IMAGES => $"{nameof(ImagesPage)}",
            TypeClinicMenu.VIDEOS => $"{nameof(VideosPage)}",
            TypeClinicMenu.LATIDOS => $"{nameof(SoundsPage)}",
            _ => throw new ArgumentException(),
        };
        return page;
    }
}
